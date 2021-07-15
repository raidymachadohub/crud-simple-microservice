using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SimpleCrudMicroservice.Domain.Context;
using SimpleCrudMicroservice.RabbitMQWeb.DTO;
using SimpleCrudMicroservice.RabbitMQWeb.Mapper;
using SimpleCrudMicroservice.Service.Interfaces;
using SimpleCrudMicroservice.Service.Services;

namespace SimpleCrudMicroservice.RabbitMQWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ListenForIntegrationEvents();
            CreateHostBuilder(args).Build().Run();
        }

        private static IEmployee _employee;

        public static IEmployee Employee
        {
            get
            {
                return _employee;
            }
        }
        
        public static void InitEmployee(IEmployee employee)
        {
            _employee = employee;
        }
        
        private static void ListenForIntegrationEvents()
        {
            var contextOptions = new DbContextOptionsBuilder<CrudContext>()
                .UseSqlServer(@"Server=localhost,5434;Initial Catalog=microservice;MultipleActiveResultSets=true;User ID=sa;Password=teste@123machado")
                .Options;
            var dbContext = new CrudContext(contextOptions); 
            
            InitEmployee(new EmployeeService(dbContext));
            
            var factory = new ConnectionFactory();
            factory.UserName = "admin";
            factory.Password = "admin";

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                var data = JObject.Parse(message);
                var type = ea.RoutingKey;
                if (type == "employee.add")
                {
                    var dto = new EmployeeDTO();
                    dto.name = data["name"].Value<string>();
                    _employee.Post(EmployeeMap.Mapper(dto));
                }
            };
            channel.BasicConsume(queue: "employee.service",
                autoAck: true,
                consumer: consumer);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
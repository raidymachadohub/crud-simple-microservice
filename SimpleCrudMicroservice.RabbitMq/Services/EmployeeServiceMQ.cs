using System;
using System.Text;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SimpleCrudMicroservice.RabbitMq.DTO;
using SimpleCrudMicroservice.RabbitMq.Mapper;
using SimpleCrudMicroservice.Service.Interfaces;

namespace SimpleCrudMicroservice.RabbitMq.Services
{
    public class EmployeeServiceMQ
    {
        private readonly IEmployee _employee;

        public EmployeeServiceMQ(IEmployee employee)
        {
            _employee = employee;
        }

        public void Run()
        {
            ListenForIntegrationEvents();
        }


        private void ListenForIntegrationEvents()
        {
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
                    dto.id = data["id"].Value<int>();
                    dto.name = data["name"].Value<string>();


                    _employee.Post(EmployeeMap.Mapper(dto));
                    
                }
            };
            channel.BasicConsume(queue: "employee.service",
                autoAck: true,
                consumer: consumer);
        }
    }
}
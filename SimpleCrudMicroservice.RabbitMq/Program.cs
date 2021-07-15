using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleCrudMicroservice.Domain.Context;
using SimpleCrudMicroservice.RabbitMq.Services;
using SimpleCrudMicroservice.Service.Interfaces;
using SimpleCrudMicroservice.Service.Services;

namespace SimpleCrudMicroservice.RabbitMq
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var employeeService = serviceProvider.GetService<EmployeeServiceMQ>();
            employeeService?.Run();
        }


        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CrudContext>(options =>
                options.UseSqlServer(
                    "Server=localhost,5434;Initial Catalog=microservice;MultipleActiveResultSets=true;User ID=sa;Password=teste@123machado"));

            services.AddTransient<IEmployee, EmployeeService>();
            services.AddTransient<EmployeeServiceMQ>();
        }
    }
}
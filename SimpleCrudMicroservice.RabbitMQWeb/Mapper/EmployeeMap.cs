using SimpleCrudMicroservice.Domain.Entity;
using SimpleCrudMicroservice.RabbitMQWeb.DTO;

namespace SimpleCrudMicroservice.RabbitMQWeb.Mapper
{
    public static class EmployeeMap
    {
        public static Employee Mapper(EmployeeDTO dto)
        {
            return new Employee()
            {
                Id = dto.id,
                Name = dto.name
            };
        }
    }
}
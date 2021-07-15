using SimpleCrudMicroservice.Domain.Entity;
using SimpleCrudMicroservice.RabbitMq.DTO;

namespace SimpleCrudMicroservice.RabbitMq.Mapper
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
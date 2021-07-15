using SimpleCrudMicroservice.Domain.Entity;
using SimpleCrudMicroservice.DTO;

namespace SimpleCrudMicroservice.Mapper
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
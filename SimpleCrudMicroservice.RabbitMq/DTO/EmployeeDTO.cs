using SimpleCrudMicroservice.Domain.Entity;

namespace SimpleCrudMicroservice.RabbitMq.DTO
{
    public class EmployeeDTO
    {
        public long id { get; set; }
        public string name { get; set; }
    }
}
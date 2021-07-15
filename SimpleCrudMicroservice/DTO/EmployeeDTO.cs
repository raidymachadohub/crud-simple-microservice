using SimpleCrudMicroservice.Domain.Entity;

namespace SimpleCrudMicroservice.DTO
{
    public class EmployeeDTO
    {
        public long id { get; set; }
        public string name { get; set; }


        public EmployeeDTO(Employee employee)
        {
            if (employee != null)
            {
                id = employee.Id;
                name = employee.Name;
            }
        }
    }
}
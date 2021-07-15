using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleCrudMicroservice.Domain.Entity;

namespace SimpleCrudMicroservice.Service.Interfaces
{
    public interface IEmployee
    {
        Task<IEnumerable<Employee>> GetAll();
        Task<Employee> GetById(int id);
        Task Put(Employee obj);
        Task<Employee> Post(Employee obj);
        Task<Employee> Delete(int id);
    }
}
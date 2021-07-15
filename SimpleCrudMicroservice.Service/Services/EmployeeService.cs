using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleCrudMicroservice.Domain.Context;
using SimpleCrudMicroservice.Domain.Entity;
using SimpleCrudMicroservice.Service.Interfaces;

namespace SimpleCrudMicroservice.Service.Services
{
    public class EmployeeService : IEmployee
    {
        private readonly CrudContext _context;

        public EmployeeService(CrudContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _context.Set<Employee>().ToListAsync();
        }

        public async Task<Employee> GetById(int id)
        {
            return await _context.Set<Employee>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Put(Employee obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<Employee> Post(Employee obj)
        {
            _context.Set<Employee>().Add(obj);
            await _context.SaveChangesAsync();

            return obj;
        }

        public async Task<Employee> Delete(int id)
        {
            var obj = await _context.Set<Employee>().FirstOrDefaultAsync(x => x.Id == id);

            if (obj == null)
            {
                throw new NotImplementedException();
            }

            _context.Set<Employee>().Remove(obj);
            await _context.SaveChangesAsync();

            return obj;
        }
    }
}
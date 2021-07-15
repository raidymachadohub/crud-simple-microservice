using Microsoft.EntityFrameworkCore;
using SimpleCrudMicroservice.Domain.Entity;

namespace SimpleCrudMicroservice.Domain.Context
{
    public class CrudContext : DbContext
    {
        public CrudContext(DbContextOptions<CrudContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Employee>().HasKey(m => m.Id);
            base.OnModelCreating(builder);
        }
        
        public DbSet<Employee> Employees { get; set; }
    }
}
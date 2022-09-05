using EmployeeApi.Models;
using Microsoft.EntityFrameworkCore;
namespace EmployeeApi.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Employee>Employees { get; set; }   
    }
}

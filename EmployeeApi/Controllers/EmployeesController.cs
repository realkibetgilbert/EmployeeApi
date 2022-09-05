using EmployeeApi.Data;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly EmployeeDbContext _employeeDbContext;  
        public EmployeesController(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }
        [HttpGet]
        public async Task <IActionResult> GetAllEmployees()
        {
           var employees=await _employeeDbContext.Employees.ToListAsync();
            return Ok(employees);
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody]Employee employeeRequest)
        {
            //if employee request as data we want to create id for it
            employeeRequest.Id = Guid.NewGuid();
            //we add a new employee
            await _employeeDbContext.Employees.AddAsync(employeeRequest);
            //we have to save chaanges
            await _employeeDbContext.SaveChangesAsync();

            return Ok(employeeRequest);
        }
    }
}

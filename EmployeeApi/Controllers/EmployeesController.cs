using EmployeeApi.Data;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeDbContext.Employees.ToListAsync();
            return Ok(employees);
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employeeRequest)
        {
            //if employee request as data we want to create id for it
            employeeRequest.Id = Guid.NewGuid();
            //we add a new employee
            await _employeeDbContext.Employees.AddAsync(employeeRequest);
            //we have to save chaanges
            await _employeeDbContext.SaveChangesAsync();

            return Ok(employeeRequest);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult>GetEmployee([FromRoute]Guid id)
        {
          var employee= await _employeeDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null)
            {
                return NotFound();
            }    
            return Ok(employee);    
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id,Employee updateEmployeeRequest)
        {
            //we first find the id
         var employee= await   _employeeDbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            //if it is there we have to update 
            employee.Name=updateEmployeeRequest.Name;   
            employee.Email=updateEmployeeRequest.Email;
            employee.Salary = updateEmployeeRequest.Salary;
            employee.Phone = updateEmployeeRequest.Phone;
            employee.Department = updateEmployeeRequest.Department;
            //and save changes to the database
           await _employeeDbContext.SaveChangesAsync();
            return Ok(employee);
        }
        //delete employee
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute]Guid id)
        {
            //we have to find the employee
            var employee = await _employeeDbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            _employeeDbContext.Employees.Remove(employee);
            await _employeeDbContext.SaveChangesAsync();
            return Ok(employee);
        }
    }
}

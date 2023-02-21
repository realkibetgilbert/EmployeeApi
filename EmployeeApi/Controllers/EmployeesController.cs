using EmployeeApi.Data;
using EmployeeApi.Exceptions;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Globalization;
using System.Text.RegularExpressions;

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
        [HttpGet("get-all-employees")]
        public async Task<IActionResult> GetAllEmployees()
        {

            throw new Exception("This is unhnsled execption");
            try
            {
                var employees = await _employeeDbContext.Employees.ToListAsync();

                return Ok(employees);
            }
            catch (Exception ex)
            {


                return BadRequest(ex.Message);
                   
            }
            finally
            {
                    
            }
          
        }
        [HttpPost("add-new-employee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employeeRequest)
        {

            //check if student name start with a number throw student nname excpetion
            try
            {
                if (Regex.IsMatch(employeeRequest.Name, @"^\d")) throw new StudentNameException("Student Name Start with a number", employeeRequest.Name);

                employeeRequest.Id = Guid.NewGuid();

                await _employeeDbContext.Employees.AddAsync(employeeRequest);

                await _employeeDbContext.SaveChangesAsync();

                return Ok(employeeRequest);
            }
           
            catch ( StudentNameException ex)
            {
                return BadRequest($"{ex.Message}"); 
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

       

        [HttpGet("get-student-by-id/{id}")]
        public async Task<IActionResult>GetEmployee(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Please provide valid id");
            //var arrayofemployee= await _employeeDbContext.Employees.ToArrayAsync(); 
              //system to index out of range exception
            //var emp = arrayofemployee[12];   

          var employee= await _employeeDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            var EmployeeName=employee.Name;
            //Null Reference Exceptioon you have a null object and you try to get property in it
            return Ok($"Student Name={EmployeeName}");
            //if (employee == null)
            //{
            //    return NotFound();
            //}    
            //return Ok(employee);    
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

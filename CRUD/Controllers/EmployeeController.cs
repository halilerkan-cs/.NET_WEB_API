using CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _employeeContext;
        public EmployeeController(EmployeeContext employeeContext)
        {
            _employeeContext= employeeContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            if(_employeeContext.Employees == null)
                return NotFound();
            return await _employeeContext.Employees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            if (_employeeContext.Employees == null)
                return NotFound();

            var emp = await _employeeContext.Employees.FindAsync(id);
            if (emp == null)
            {
                return NotFound();
            }
            else
            {
                return emp;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _employeeContext.Employees.Add(employee);
            await _employeeContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new {id = employee.ID}, employee);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutEmployee(int id, Employee employee)
        {
            if(id != employee.ID)
            {
                return BadRequest();
            }

            _employeeContext.Employees.Entry(employee).State = EntityState.Modified;
            try
            {
                await _employeeContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) { throw; }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            if(_employeeContext.Employees == null) return NotFound();

            var emp = await _employeeContext.Employees.FindAsync(id);

            if (emp == null) { return NotFound();}

            _employeeContext.Employees.Remove(emp);
            await _employeeContext.SaveChangesAsync();

            return Ok();
        }
    }
}

#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_CommerceContinental.Server.Data;
using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeeController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Employee
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        return await _context.Employees.AsNoTracking()
                                       .Include(x => x.Branch)
                                       .Where(x => x.PhoneNo != "07068716813")
                                       .ToListAsync();
    }

    // GET: api/Employee/UsernameExist/{Username}
    [HttpGet("UsernameExist/{username}")]
    public async Task<ActionResult<bool>> CheckUserEmployee(string username)
    {
        return await _context.EmployeeAccounts.AnyAsync(x => x.Username == username);            
    }

        
    
    // GET: api/Employee/WithNoSalary
    // [HttpGet("WithNoSalary")]
    // public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesWithNoSalary()
    // {
    //     var employees = await _context.Employees.AsNoTracking()
    //                                             .Include(x => x.Salaries)
    //                                             .Include(x => x.SalaryAdvances)
    //                                             .Include(x => x.SalaryBonus)
    //                                             .Include(x => x.SalaryPenalties)
    //                                             .Where(x => !x.Salaries.Any(c => c.SalMonth == DateTime.Now.Month && c.SalYear == DateTime.Now.Year) && x.PhoneNo != "07068716813")                                                    
    //                                             .OrderBy(x => x.FirstName)
    //                                             .ThenBy(x => x.LastName)
    //                                             .ThenByDescending(x => x.Salary)
    //                                             .ToListAsync();
    //     if (employees is null)
    //     {
    //         return Array.Empty<Employee>();
    //     }
    //     return employees;
    // }

    // GET: api/Employee/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(Guid id)
    {
        var employee = await _context.Employees.AsSplitQuery()
                                               .Include(x => x.Branch)
                                               .Include(x => x.EmployeeAccount)
                                               .ThenInclude(x => x.Shop)
                                               .FirstOrDefaultAsync(x => x.Id == id);

        if (employee == null)
        {
            return NotFound();
        }

        return employee;
    }

    // PUT: api/Employee/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(Guid id, Employee employee)
    {
        if (id != employee.Id)
        {
            return BadRequest();
        }

        _context.Entry(employee).State = EntityState.Modified;
        _context.EmployeeAccounts.Update(employee.EmployeeAccount);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmployeeExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Employee
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
    }

    // POST: api/Employee/Sync
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Route("sync")]
    public async Task<ActionResult<Employee>> PostEmployees(Employee employee)
    {
        if (!EmployeeExists(employee.Id))
            _context.Employees.Add(employee);
        else
            _context.Employees.Update(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
    }

    
    // POST: api/Employee/Account/Sync
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Route("registeraccount")]
    public async Task<ActionResult<Employee>> PostEmployeeAccount(EmployeeAccount employee)
    {
        _context.EmployeeAccounts.Update(employee);                
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEmployee", new { id = employee.EmpID }, employee);
    }

    // POST: api/Employee/Account/Sync
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Route("account/sync")]
    public async Task<ActionResult<Employee>> PostEmployeesAccount(EmployeeAccount employee)
    {
        if (!EmployeeAccountExists(employee.Id))
            _context.EmployeeAccounts.Add(employee);
        else
            _context.EmployeeAccounts.Update(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
    }

    // DELETE: api/Employee/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(Guid id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EmployeeExists(Guid id)
    {
        return _context.Employees.Any(e => e.Id == id);
    }

    private bool EmployeeAccountExists(Guid id)
    {
        return _context.EmployeeAccounts.Any(e => e.Id == id);
    }
}

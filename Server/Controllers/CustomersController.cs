using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_CommerceContinental.Server.Data;
using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Server.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("existcustomer/{phone}")]
    public async Task<ActionResult<Customer>> ExistCustomer(string phone)
    {
        return await _context.Customers.FirstOrDefaultAsync(x => x.PhoneNo == phone) ?? new Customer();
    }

    // GET: api/Customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        return await _context.Customers.OrderBy(o => o.CustomerName)
                                       .ToListAsync();
    }
        

    [HttpGet("topproducts")]
    public async Task<ActionResult<Dictionary<string, int>>> TopProductsCustomer()
    {
        var customer = await _context.Customers.AsSplitQuery().Include(x => x.Orders).OrderByDescending(x => x.Orders.Count).FirstOrDefaultAsync();
        return new Dictionary<string, int>()
        {
            { customer!.CustomerName!, customer.Orders.Count } 
        };
    }

    [HttpGet("recents/{date}")]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(DateOnly date)
    {
        return await _context.Customers.OrderBy(o => o.CustomerName)
                                       .Where(x => DateOnly.FromDateTime(x.ModifiedDate!.Value) >= date)
                                       .ToListAsync();
    }
    // GET: api/Customers/outstandingbalance
    [HttpGet("outstandingbalance/{id}")]
    public async Task<ActionResult<decimal>> GetOutstandingBalance(Guid id)
    {
        var orders = await _context.Orders.Where(x => x.CustomerID == id).ToArrayAsync();
        return orders.Sum(x => x.Balance);
    }

    [HttpGet]
    [Route("only")]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersOnly()
    {
        return await _context.Customers.OrderBy(x => x.CustomerName).ToListAsync();
    }

    // GET: api/Customers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(Guid id)
    {
        var customer = await _context.Customers.AsSplitQuery()
                                               .Include(x => x.Orders.OrderByDescending(x => x.InvoiceNo))                                               
                                               .FirstOrDefaultAsync(x => x.CustomerId == id);

        if (customer == null)
        {
           return NotFound();
        }

       return customer;
    }
    
    // PUT: api/Customers/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(Guid id, Customer customer)
    {
        if (id != customer.CustomerId)
        {
            return BadRequest();
        }

        _context.Entry(customer).State = EntityState.Modified;        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CustomerExists(id))
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
    
    // POST: api/Customers
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
    {
        _context.Customers.Add(customer);            
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
    }

    // POST: api/Customers/Sync
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Route("sync")]
    public async Task<ActionResult<Customer>> PostCustomers(Customer customer)
    {
        if (!CustomerExists(customer.CustomerId))
            _context.Customers.Add(customer);
        else
            _context.Customers.Update(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
    }
    

    [HttpPost("bulkinsert")]
    public async Task<ActionResult> PostCustomers(List<Customer> customers)
    {
        try
        {
            await _context.Customers.AddRangeAsync(customers);
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpGet("phone/{number}")]
    public async Task<ActionResult<Customer>> CheckPhone(string number)
    {
        return await _context.Customers.Where(x => x.PhoneNo == number).FirstOrDefaultAsync() ?? new Customer();
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return NotFound();
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CustomerExists(Guid id)
    {
        return _context.Customers.Any(e => e.CustomerId == id);
    }
    
}

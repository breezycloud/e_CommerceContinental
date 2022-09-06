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
public class BanksController : ControllerBase
{
    private readonly AppDbContext _context;

    public BanksController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Banks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bank>>> GetBanks()
    {
        return await _context.Banks.OrderBy(i => i.BankName).ToListAsync();
    }

    // GET: api/Banks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Bank>> GetBank(Guid id)
    {
        var bank = await _context.Banks.FindAsync(id);

        if (bank == null)
        {
            return NotFound();
        }

        return bank;
    }

    // PUT: api/Banks/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBank(Guid id, Bank bank)
    {
        if (id != bank.Id)
        {
            return BadRequest();
        }

        _context.Entry(bank).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BankExists(id))
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

    // POST: api/Banks
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Bank>> PostBank(Bank bank)
    {
        _context.Banks.Add(bank);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBank", new { id = bank.Id }, bank);
    }

    // POST: api/Banks
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Route("sync")]
    public async Task<ActionResult<Bank>> PostBanks(Bank bank)
    {
        if (!BankExists(bank.Id))
            _context.Banks.Add(bank);
        else
            _context.Banks.Update(bank);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // DELETE: api/Banks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBank(Guid id)
    {
        var bank = await _context.Banks.FindAsync(id);
        if (bank == null)
        {
            return NotFound();
        }

        _context.Banks.Remove(bank);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool BankExists(Guid id)
    {
        return _context.Banks.Any(e => e.Id == id);
    }
}

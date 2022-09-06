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
public class BranchesController : ControllerBase
{
    private readonly AppDbContext _context;

    public BranchesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Branches
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Branch>>> GetBranches()
    {
        return await _context.Branches.AsSplitQuery()
                                      .Include(x => x.Shops)
                                      .ToListAsync();
    }

    // GET: api/Branches/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Branch>> GetBranch(Guid id)
    {
        var branch = await _context.Branches.FindAsync(id);

        if (branch == null)
        {
            return NotFound();
        }

        return branch;
    }

    // PUT: api/Branches/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBranch(Guid id, Branch branch)
    {
        if (id != branch.Id)
        {
            return BadRequest();
        }

        _context.Entry(branch).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BranchExists(id))
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

    // POST: api/Branches
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Branch>> PostBranch(Branch branch)
    {
        _context.Branches.Add(branch);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBranch", new { id = branch.Id }, branch);
    }

    // POST: api/Branches
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Route("sync")]
    public async Task<ActionResult<Branch>> PostBranchs(Branch branch)
    {
        if (BranchExists(branch.Id))
            _context.Branches.Add(branch);
        else
            _context.Branches.Update(branch);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBranch", new { id = branch.Id }, branch);
    }

    // DELETE: api/Branches/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBranch(Guid id)
    {
        var branch = await _context.Branches.FindAsync(id);
        if (branch == null)
        {
            return NotFound();
        }

        _context.Branches.Remove(branch);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool BranchExists(Guid id)
    {
        return _context.Branches.Any(e => e.Id == id);
    }
}

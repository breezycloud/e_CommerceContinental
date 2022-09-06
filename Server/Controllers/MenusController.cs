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

// [Authorize]
[Route("api/[controller]")]
[ApiController]

public class MenusController : ControllerBase
{
    private readonly AppDbContext _context;

    public MenusController(AppDbContext context)
    {
        _context = context;
    }

    //GET: api/Menus
    public async Task<ActionResult<IEnumerable<Menu>>> GetMenus()
    {
        return await _context.Menus.AsSplitQuery()
                                   .Include(x => x.SubMenus)
                                   .OrderBy(i => i.Order)
                                   .ToListAsync();
    }

    //GET: api/Menus/id
    [HttpGet("{id}")]
    public async Task<ActionResult<Menu>> GetMenu(Guid id)
    {
        return await _context.Menus.AsSplitQuery()
                                   .Include(x => x.SubMenus)
                                   .Where(x => x.Id == id)
                                   .OrderBy(i => i.Order)
                                   .FirstOrDefaultAsync() ?? new Menu();
    }

    //GET: api/SubMenu/id
    [HttpGet("SubMenu/{id}")]
    public async Task<ActionResult<SubMenu>> GetSubMenu(Guid id)
    {
        return await _context.SubMenus.AsSplitQuery()
                                      .Include(x => x.Menu)
                                      .Where(x => x.Id == id)
                                      .OrderBy(i => i.Order)
                                      .FirstOrDefaultAsync() ?? new SubMenu();
    }

    // PUT: api/Menus/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMenu(Guid id, Menu menu)
    {
        if (id != menu.Id)
        {
            return BadRequest();
        }

        _context.Entry(menu).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return NoContent();
    }

    // PUT: api/Menus/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("SubMenu/{id}")]
    public async Task<IActionResult> PutSubMenu(Guid id, SubMenu menu)
    {
        if (id != menu.Id)
        {
            return BadRequest();
        }

        _context.Entry(menu).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return NoContent();
    }

    // POST: api/Menus/SubMenu
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("SubMenu")]
    public async Task<ActionResult<Menu>> PostSubMenu(SubMenu menu)
    {
        menu.Menu = null;
        _context.SubMenus.Add(menu);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetSubMenu", new { id = menu.Id }, menu);
    }
    // POST: api/Menus
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Menu>> PostMenu(Menu menu)
    {
        _context.Menus.Add(menu);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMenu", new { id = menu.Id }, menu);
    }

    // DELETE: api/Menus/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMenu(Guid id)
    {
        var menu = await _context.Menus.FindAsync(id);
        if (menu == null)
        {
            return NotFound();
        }

        _context.Menus.Remove(menu);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Menus/SubMenu/5
    [HttpDelete("SubMenu/{id}")]
    public async Task<IActionResult> DeleteSubMenu(Guid id)
    {
        var menu = await _context.SubMenus.FindAsync(id);
        if (menu == null)
        {
            return NotFound();
        }

        _context.SubMenus.Remove(menu);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
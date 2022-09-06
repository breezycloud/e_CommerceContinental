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
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Products
    [HttpGet("related")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
    {
        return await _context.Products.Include(x => x.Shop).Include(x => x.Category).AsSplitQuery().ToListAsync();
    }
    // GET: api/Products
    [HttpGet("existproductcode/{code}")]
    public async Task<ActionResult<Product>> GetProductWithCode(string code)
    {
        return await _context.Products.Where(x => x.Code == code).FirstOrDefaultAsync() ?? new Product();
    }
    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(Guid id)
    {
        var product = await _context.Products.Include(x => x.Stocks).FirstOrDefaultAsync(x => x.Id ==id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpGet("fabrics")]
    public async Task<ActionResult<IEnumerable<Product>>> GetFabrics() =>
        await _context.Products.Include(x => x.Category).Where(x => x.Category.CategoryName == "Fabrics").Select(n => new Product{
            Id = n.Id,
            ProductName = n.ProductName,
            Description = n.Description,
            CategoryID = n.CategoryID,
            Barcode = n.Barcode,
        }).ToListAsync();

    // GET: api/Products/ByBranch/5
    [HttpGet("byBranch/{id}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByBranch(Guid id)
    {
        var products = await _context.Products.Where(x => x.ShopID == id && x.StocksOnHand >= 1).ToListAsync();

        if (products == null)
        {
            return NotFound();
        }

        return products;
    }

    // GET: api/Products/ByShop/5
    [HttpGet("byShop/{id}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByShop(Guid id)
    {
        var products = await _context.Products.AsSplitQuery()
                                              .Include(x => x.Category)
                                              .Where(x => x.ShopID == id && x.StocksOnHand >= 1)
                                              .ToListAsync();

        if (products == null)
        {
            return NotFound();
        }

        return products;
    }

    // PUT: api/Products/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(Guid id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(id))
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

    // PUT: api/Products/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("many")]
    public async Task<IActionResult> PutProducts(List<Product> products)
    {
        products.ForEach((x) => { x.Category = null; x.Stocks = null; x.Shop = null; });
        _context.Products.UpdateRange(products);
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

    // POST: api/Products
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProduct", new { id = product.Id }, product);
    }

    // POST: api/Products/Sync
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Route("sync")]
    public async Task<ActionResult<Product>> PostProducts(Product product)
    {
        if (!ProductExists(product.Id))            
            _context.Products.Add(product);
        else
            _context.Products.Update(product);

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProduct", new { id = product.Id }, product);
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(Guid id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}

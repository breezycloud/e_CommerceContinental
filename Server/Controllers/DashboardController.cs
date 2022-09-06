#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_CommerceContinental.Server.Data;
using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Dashboard/GetData
    [HttpGet("getdata")]
    public async Task<ActionResult<DashboardResponse>> GetData() => new DashboardResponse()
    {
        TopChartData = new TopChartData()
        {
            TotalEmployees = await _context.Employees.Where(x => x.PhoneNo != "07068716813").CountAsync(),
            TotalBranches = await _context.Branches.CountAsync(),
            TotalShops = await _context.Shops.CountAsync(),
            TotalCustomers = await _context.Customers.CountAsync(),
            TotalProducts = await _context.Products.Where(x => x.StocksOnHand >= 1).CountAsync(),
            TotalSales = await _context.Orders.CountAsync()
        },
        OrdersLineChart = await _context.Orders.GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).Select(y => new OrderLineChart
        {
            Year = y.Key.Year,
            Month = y.Key.Month,
            TotalSales = y.Count(x => x.TransactionDate.Month == y.Key.Month && x.TransactionDate.Year == y.Key.Year)
        }).ToArrayAsync(),
        ProductsPieCharts = await _context.OrderDetails.Include(x => x.Product).GroupBy(x => x.ProductID).Select(x => new ProductsPieChart()
        {
            Product = x.Select(i => i.Product.ProductName).FirstOrDefault(),
            TotalSales = x.Select(i => i.Id).Count()
        }).OrderByDescending(x => x.TotalSales).Take(10).ToArrayAsync(),
        BranchSales = await _context.Orders.Include(x => x.Employee).ThenInclude(x => x.Branch).GroupBy(x => x.EmpID).Select(y => new BranchSales()
        {
            BranchName = y.Select(x => x.Employee.Branch.BranchName).FirstOrDefault(),
            TotalSales = y.Select(i => i.Id).Count()
        }).ToArrayAsync(),
        BranchShopSales = await _context.Orders.AsSplitQuery()
                                               .Include(x => x.Employee)
                                               .ThenInclude(x => x.Branch)
                                               .Include(x => x.Employee)
                                               .ThenInclude(x => x.EmployeeAccount)
                                               .ThenInclude(x => x.Shop)
                                               .GroupBy(x => x.EmpID).Select(y => new BranchShopSales()
                                                {
                                                    BranchName = y.Select(i => i.Employee.Branch.BranchName).FirstOrDefault(),
                                                    ShopName = y.Select(i => i.Employee.EmployeeAccount.Shop.ShopName).FirstOrDefault(),
                                                    TotalSales = y.Select(i => i.Id).Count()
                                                }).ToArrayAsync()
    };

    // GET: api/Dashboard/GetData/6
    [HttpGet("getdata/{id}")]
    public async Task<ActionResult<DashboardResponse>> GetData(Guid id) => new DashboardResponse()
    {
        TopChartData = new TopChartData()
        {
            TotalEmployees = await _context.Employees.Where(x => x.PhoneNo != "07068716813").CountAsync(),
            TotalBranches = await _context.Branches.CountAsync(),
            TotalShops = await _context.Shops.CountAsync(),
            TotalCustomers = await _context.Customers.CountAsync(),
            TotalProducts = await _context.Products.Where(x => x.StocksOnHand >= 1).CountAsync(),
            TotalSales = await _context.Orders.AsSplitQuery().Include(x => x.Employee).ThenInclude(x => x.EmployeeAccount).Where(x => x.Employee.EmployeeAccount.ShopID == id).CountAsync()
        },
        OrdersLineChart = await _context.Orders.AsSplitQuery().Include(x => x.Employee).ThenInclude(x => x.EmployeeAccount).Where(x => x.Employee.EmployeeAccount.ShopID == id).GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).Select(y => new OrderLineChart
        {
            Year = y.Key.Year,
            Month = y.Key.Month,
            TotalSales = y.Count(x => x.TransactionDate.Month == y.Key.Month && x.TransactionDate.Year == y.Key.Year)
        }).ToArrayAsync(),
        ProductsPieCharts = await _context.OrderDetails.Include(x => x.Product).Where(x => x.Product.ShopID == id).GroupBy(x => x.ProductID).Select(x => new ProductsPieChart()
        {
            Product = x.Select(i => i.Product.ProductName).FirstOrDefault(),
            TotalSales = x.Select(i => i.Id).Count()
        }).OrderByDescending(x => x.TotalSales).Take(5).ToArrayAsync(),
        BranchSales = await _context.Orders.Include(x => x.Employee).ThenInclude(x => x.Branch).GroupBy(x => x.EmpID).Select(y => new BranchSales()
        {
            BranchName = y.Select(x => x.Employee.Branch.BranchName).FirstOrDefault(),
            TotalSales = y.Select(i => i.Id).Count()
        }).ToArrayAsync(),
        BranchShopSales = await _context.Orders.AsSplitQuery()
                                               .Include(x => x.Employee)
                                               .ThenInclude(x => x.Branch)
                                               .Include(x => x.Employee)
                                               .ThenInclude(x => x.EmployeeAccount)
                                               .ThenInclude(x => x.Shop)
                                               .GroupBy(x => x.EmpID).Select(y => new BranchShopSales()
                                                {
                                                    BranchName = y.Select(i => i.Employee.Branch.BranchName).FirstOrDefault(),
                                                    ShopName = y.Select(i => i.Employee.EmployeeAccount.Shop.ShopName).FirstOrDefault(),
                                                    TotalSales = y.Select(i => i.Id).Count()
                                                }).ToArrayAsync()
    };

    // GET: api/Dashboard/TotalEmployees
    [HttpGet("totalemployees")]
    public async Task<ActionResult<long>> TotalEmployees()
    {
        return await _context.Employees.Where(x => x.PhoneNo != "07068716813").CountAsync();            
    }

    // GET: api/Dashboard/TotalCustomers
    [HttpGet("totalcustomers")]
    public async Task<ActionResult<long>> TotalCustomers()
    {
        return await _context.Customers.CountAsync();
    }

    // GET: api/Dashboard/TotalProducts
    [HttpGet("totalproducts")]
    public async Task<ActionResult<long>> TotalProducts()
    {
        return await _context.Products.Where(x => x.StocksOnHand >= 1).CountAsync();
    }

    // GET: api/Dashboard/TotalBranches
    [HttpGet("totalbranch")]
    public async Task<ActionResult<long>> TotalBranch()
    {
        return await _context.Branches.CountAsync();
    }        

    // GET: api/Dashboard/TotalSales
    [HttpGet("totalsales")]
    public async Task<ActionResult<long>> TotalSales()
    {
        return await _context.Orders.CountAsync();
    }        
    
    // GET: api/Dashboard/ShopTotalSales/id
    [HttpGet("shoptotalsales/{id}")]
    public async Task<ActionResult<long>> ShopTotalSales(Guid id)
    {
        return await _context.Orders.AsSplitQuery()
                                    .Include(x => x.OrderDetails)
                                    .ThenInclude(x => x.Product.ShopID == id)
                                    .CountAsync();
    }        

    // GET: api/Dashboard/Shops
    [HttpGet("totalshops")]
    public async Task<ActionResult<long>> TotalShops()
    {
        return await _context.Shops.CountAsync();
    }        
}   
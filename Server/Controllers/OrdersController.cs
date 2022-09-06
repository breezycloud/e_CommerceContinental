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
using Microsoft.AspNetCore.Authorization;
using e_CommerceContinental.Server.Util;
using Microsoft.Reporting.NETCore;
using System.Reflection;

namespace e_CommerceContinental.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;
    private IConverter _converter;

    public OrdersController(AppDbContext context, IConverter converter)
    {
        _context = context;
        _converter = converter;
    }

    // GET: api/ProductOrder
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetProductOrder()
    {
        return await _context.Orders.ToListAsync();
    }

    [Authorize]
    [HttpGet("GetPreviousPayment/{id}")]
    public async Task<ActionResult<List<OrderPayment>>> GetPreviousPayment(Guid id)
    {
        return await _context.OrderPayments.Where(x => x.OrderID == id)
                                                  .OrderByDescending(x => x.PaymentDate)
                                                  .ToListAsync();
    }

    // GET: api/ProductOrders/CustomerWithOrder
    [Authorize]
    [HttpGet("CustomerWithOrders")]
    public async Task<ActionResult<IEnumerable<CustomerWithOrder>>> GetCustomerOrders()
    {
        return await _context.Orders.Include(c => c.Customer).Include(u => u.Employee)
                                          .Include(x => x.OrderDetails)
                                          .ThenInclude(x => x.Product)
                                          .OrderByDescending(i => i.InvoiceNo)
                                          .Select(e => new CustomerWithOrder()
                                            {
                                                OrderId = e.Id,
                                                Customer = e.Customer.CustomerName,
                                                PhoneNo = e.Customer.PhoneNo,
                                                Email = e.Customer.Email,
                                                ContactAddress = e.Customer.ContactAddress,
                                                OfficeAddress = e.Customer.OfficeAddress,
                                                ReceiptNo = e.InvoiceNo.ToString().PadLeft(4, '0'),
                                                OrderDate = e.TransactionDate,
                                                TotalAmount = e.TotalAmount,
                                                Discount = e.Discount,
                                                AmountPaid = e.AmountPaid,
                                                VAT = e.VAT,
                                                PaymentMode = e.PaymentMode,  
                                                OrderDetails = e.OrderDetails.ToList().Select(y => new ReceiptItem
                                                {
                                                    Item = y.Product.ProductName,
                                                    TotalAmount = y.ItemPrice,
                                                    UnitPrice = y.Product.UnitPrice,
                                                    Quantity = (int)y.Quantity,
                                                }).ToList()
                                            }).ToListAsync();            
    }

    // GET: api/ProductOrders/CustomerWithOrder
    [Authorize]
    [HttpGet("BranchCustomerWithOrders/{BranchID}")]
    public async Task<ActionResult<IEnumerable<CustomerWithOrder>>> GetBranchCustomerOrders(Guid BranchID)
    {
        return await _context.Orders.Include(c => c.Customer).Include(u => u.Employee)
                                          .Include(x => x.OrderDetails)
                                          .ThenInclude(x => x.Product)
                                          .OrderByDescending(i => i.InvoiceNo)
                                          .Where(x => x.Employee.BranchID == BranchID)
                                          .Select(e => new CustomerWithOrder()
                                            {
                                                OrderId = e.Id,
                                                Customer = e.Customer.CustomerName,
                                                PhoneNo = e.Customer.PhoneNo,
                                                Email = e.Customer.Email,
                                                ContactAddress = e.Customer.ContactAddress,
                                                OfficeAddress = e.Customer.OfficeAddress,
                                                ReceiptNo = e.InvoiceNo.ToString().PadLeft(4, '0'),
                                                OrderDate = e.TransactionDate,
                                                TotalAmount = e.TotalAmount,
                                                Discount = e.Discount,
                                                AmountPaid = e.AmountPaid,
                                                VAT = e.VAT,
                                                PaymentMode = e.PaymentMode,  
                                                OrderDetails = e.OrderDetails.ToList().Select(y => new ReceiptItem
                                                {
                                                    Item = y.Product.ProductName,
                                                    TotalAmount = y.ItemPrice,
                                                    UnitPrice = y.Product.UnitPrice,
                                                    Quantity = (int)y.Quantity,
                                                }).ToList()
                                            }).ToListAsync();            
    }

    [HttpGet("orderdetails")]
    public async Task<ActionResult<IEnumerable<OrderDetails>>> GetOrderDetails()
    {
        return await _context.OrderDetails.Include(x => x.Product).AsSplitQuery().ToListAsync();
    }
    // GET: api/ProductOrder/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetProductOrder(Guid id)
    {
        var productOrder = await _context.Orders.AsSplitQuery()
                                                .Include(c => c.Customer)
                                                .Include(o => o.Employee)                                                
                                                .ThenInclude(b => b.Branch)
                                                .Include(e => e.Employee.EmployeeAccount)
                                                .ThenInclude(s => s.Shop)
                                                .Include(u => u.OrderDetails)
                                                .ThenInclude(o => o.Product)
                                                .ThenInclude(c => c.Category)
                                                .Include(p => p.Payments.OrderByDescending(x => x.PaymentDate))
                                                .Where(i => i.Id == id)
                                                .FirstOrDefaultAsync();

        if (productOrder == null)
        {
            return NotFound();
        }

        return productOrder;
    }

    [Authorize]
    [HttpGet("receiptno")]
    public async Task<ActionResult<int>> GetReceiptNo()
    {
        var result = await _context.Orders.Select(i => i.InvoiceNo).CountAsync();
        result += 1;

        return result;
    }

    // PUT: api/ProductOrder/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProductOrder(Guid id, Order productOrder)
    {
        if (id != productOrder.Id)
        {
            return BadRequest();
        }

        _context.Entry(productOrder).State = EntityState.Modified;            
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductOrderExists(id))
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

    [Authorize]
    [HttpGet("GetReceipt/{id}")]
    public async Task<IActionResult> GetReceipt(Guid id)
    {
        var order = await _context.Orders.AsSplitQuery()
                                         .Include(c => c.Customer)
                                         .Include(u => u.Employee)
                                         .ThenInclude(u => u.Branch)
                                         .Include(o => o.OrderDetails)
                                         .ThenInclude(o => o.Product)
                                         .Where(r => r.Id == id)
                                         .OrderBy(i => i.InvoiceNo)
                                         .FirstOrDefaultAsync();
        
        var receipts = await GetReceiptDetails(order);

        using var fs = Assembly.GetExecutingAssembly().GetManifestResourceStream("e_CommerceContinental.Server.Reports.Receipt.rdlc");
        LocalReport report = new();
        report.LoadReportDefinition(fs);
        report.SetParameters(new ReportParameter[]
        {
            new ReportParameter("BranchAddress", order.Employee.Branch.BranchAddress),
            new ReportParameter("BranchTel", string.Join(',', new string[] {order.Employee.Branch.PhoneNo1, order.Employee.Branch.PhoneNo2}))
        });
        report.DataSources.Add(new ReportDataSource("Order", receipts));
        byte[] pdf = report.Render("HTML5", DeviceInfo());

        // using var ms = new FileStream($@"{receiptPath}\receipt.html", FileMode.Create, FileAccess.Write);
        // await ms.WriteAsync(pdf, 0, pdf.Length);
        return File(pdf, "text/html", "report.html");

    }
    // POST: api/ProductOrder
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Order>> PostProductOrder(Order productOrder)
    {            
        _context.Orders.Add(productOrder);            
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProductOrder", new { id = productOrder.Id }, productOrder);
    }

    [Authorize]
    [HttpPost("UpdatePayment")]
    public async Task UpdatePayment(OrderPayment model)
    {
        _context.OrderPayments.Add(model);
        await _context.SaveChangesAsync();
    }

    // POST: api/ProductOrder/Sync
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Route("sync")]
    public async Task<ActionResult> PostProductOrders(Order productOrder)
    {
        if (!ProductOrderExists(productOrder.Id))
            _context.Orders.Update(productOrder);
        else
            _context.Orders.Add(productOrder);                

        await _context.SaveChangesAsync();

        return Ok();
    }

    // POST: api/ProductOrder/Sync
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Route("details/sync")]
    public async Task<ActionResult> PostProductOrderDetails(OrderDetails productOrder)
    {
        if (!ProductOrderDetailExists(productOrder.Id))
            _context.OrderDetails.Add(productOrder);
        else
            _context.OrderDetails.Update(productOrder);                

        await _context.SaveChangesAsync();

        return Ok();
    }

    // DELETE: api/ProductOrder/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductOrder(Guid id)
    {
        var productOrder = await _context.Orders.FindAsync(id);
        if (productOrder == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(productOrder);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductOrderExists(Guid id)
    {
        return _context.Orders.Any(e => e.Id == id);
    }

    private bool ProductOrderDetailExists(Guid id)
    {
        return _context.OrderDetails.Any(e => e.Id == id);
    }
    private async Task<List<Receipt>> GetReceiptDetailss(Order model)
    {
        var qrCode = await _converter.ConvertImageToByte(model.Id);
        List<Receipt> receipt = new();
        var orderDetails = model.OrderDetails.GroupBy(x => x.ProductID, (x, y) => new
        {
            Quantity = y.Sum(z => z.Quantity),
            ServiceName = y.Select(c => c.Product.ProductName).FirstOrDefault(),
            Cost = y.Sum(z => z.UnitPrice)
        }).ToList();
        foreach (var item in orderDetails)
        {
            receipt.Add(new Receipt()
            {
                ReceiptNo = model.InvoiceNo.ToString().PadLeft(4, '0'),
                CustomerName = model.Customer.CustomerName,
                OrderDate = model.TransactionDate.ToShortDateString(),
                TotalAmount = model.TotalAmount,
                AmountPaid = model.AmountPaid,
                Cashier = model.Employee.FullName,
                CatalogName = item.ServiceName,
                Cost = item.Cost,
                Discount = model.Discount,
                Quantity = item.Quantity,
                QrCode = qrCode
            });
        }
        return receipt;
    }

    [Authorize]
    [HttpPost("GetOrderReceipt")]
    public async Task<IActionResult> OrderReceipt(List<Receipt> receipts)
    {            
        Guid OrderID = receipts.Select(x => x.OrderID).FirstOrDefault();
        var qrCode = await _converter.ConvertImageToByte(OrderID);
        receipts.ForEach((i) => { i.QrCode = qrCode; });
        var order = await _context.Orders.AsSplitQuery()
                                         .Include(u => u.Employee)
                                         .ThenInclude(b => b.Branch)
                                         .Where(x => x.Id == OrderID)
                                         .FirstOrDefaultAsync();
        using var fs = Assembly.GetExecutingAssembly().GetManifestResourceStream("e_CommerceContinental.Server.Reports.Receipt.rdlc");
        LocalReport report = new();
        report.LoadReportDefinition(fs);
        report.DataSources.Add(new ReportDataSource("Order", receipts));
        report.SetParameters(new ReportParameter[]
        {
            new ReportParameter("BranchAddress", order.Employee.Branch.BranchAddress),
            new ReportParameter("BranchTel", string.Join(',', new string[] {order.Employee.Branch.PhoneNo1, order.Employee.Branch.PhoneNo2}))
        });
        byte[] pdf = report.Render("HTML5", DeviceInfo());
        return File(pdf, "text/html", "report.html");
    }

    private async Task<List<Receipt>> GetReceiptDetails(Order model)
    {
        var qrCode = await _converter.ConvertImageToByte(model.Id);
        List<Receipt> receipt = new();
        var orderDetails = model.OrderDetails.GroupBy(x => x.ProductID, (x, y) => new
        {
            Quantity = y.Sum(z => z.Quantity),
            ServiceName = y.Select(c => c.Product.ProductName).FirstOrDefault(),
            Cost = y.Sum(z => z.ItemPrice)
        }).ToList();
        foreach (var item in orderDetails)
        {
            receipt.Add(new Receipt()
            {
                ReceiptNo = model.InvoiceNo.ToString().PadLeft(4, '0'),
                CustomerName = model.Customer.CustomerName,
                OrderDate = model.TransactionDate.ToShortDateString(),
                TotalAmount = model.TotalAmount,
                AmountPaid = model.AmountPaid,
                Cashier = model.Employee.FullName,
                CatalogName = item.ServiceName,
                Cost = item.Cost,
                Discount = model.Discount,
                Quantity = item.Quantity,
                QrCode = qrCode
            });
        }
        return receipt;
    }



    private string DeviceInfo() =>            
            @"<DeviceInfo>
                    <OutputFormat>EMF</OutputFormat>
                    <PageWidth>3.149in</PageWidth>
                    <PageHeight>0in</PageHeight>
                    <MarginTop>0</MarginTop>
                    <MarginLeft>0</MarginLeft>
                    <MarginRight>0</MarginRight>
                    <MarginBottom>0</MarginBottom>
                </DeviceInfo>";
}

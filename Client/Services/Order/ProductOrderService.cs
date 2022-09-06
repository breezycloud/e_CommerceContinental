using System.Net.Http.Json;
using Blazored.LocalStorage;
using e_CommerceContinental.Client.Data;
using e_CommerceContinental.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace e_CommerceContinental.Client.Services;

public class ProductOrderService : IProductOrderService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    private readonly ILocalStorageService _localStorage;
    private readonly DbContextProvider _context;
    public ProductOrderService(HttpClient http, IJSRuntime js, 
        DbContextProvider context, ILocalStorageService localStorage)
    {
        _http = http;
        _js = js;
        _context = context;
        _localStorage = localStorage;
    }

    public async Task<bool> AddOrder(Order model)
    {
        bool statusCode = false;
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))
            {
                using var response = await _http.PostAsJsonAsync("api/orders", model);
                response.EnsureSuccessStatusCode();
                statusCode = response.IsSuccessStatusCode;
            }                
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // await db.ProductOrder.AddAsync(model);
                // var rowsAffected = await db.SaveChangesAsync();
                // if (rowsAffected >= 1)
                // {
                //     var local = await _localStorage.GetItemAsync<int>("localChanges");
                //     local++;
                //     await _localStorage.SetItemAsync<int>("localChanges", local);
                //     statusCode = true;
                // }
                // else
                    statusCode = false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return statusCode;                     
    }

    public async Task<bool> DeleteOrder(Order model)
    {
        using var response = await _http.DeleteAsync($"api/orders/{model.Id}");
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async ValueTask<CustomerWithOrder[]> GetCustomerWithOrders()
    {
        CustomerWithOrder[] orders = Array.Empty<CustomerWithOrder>();
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))
            {
                orders = await _http.GetFromJsonAsync<CustomerWithOrder[]>("api/orders/customerwithorders") ?? Array.Empty<CustomerWithOrder>();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // orders = await db.ProductOrder.AsSplitQuery()
                //                               .Include(c => c.Customer)
                //                               .Include(u => u.Employee)
                //                               .Include(x => x.ProductOrderDetails)
                //                               .ThenInclude(x => x.Product)
                //                               .OrderByDescending(i => i.InvoiceNo)
                //                               .Select(e => new CustomerWithOrder()
                //                                 {
                //                                     OrderId = e.Id,
                //                                     Customer = e.Customer.CustomerName,
                //                                     PhoneNo = e.Customer.PhoneNo,
                //                                     Email = e.Customer.Email,
                //                                     ContactAddress = e.Customer.ContactAddress,
                //                                     OfficeAddress = e.Customer.OfficeAddress,
                //                                     ReceiptNo = e.InvoiceNo.ToString().PadLeft(4, '0'),
                //                                     OrderDate = e.TransactionDate,
                //                                     TotalAmount = e.TotalAmount,
                //                                     Discount = e.Discount,
                //                                     AmountPaid = e.AmountPaid,
                //                                     VAT = e.VAT,
                //                                     PaymentMode = e.PaymentMode,  
                //                                     OrderDetails = e.ProductOrderDetails.ToList().Select(y => new ReceiptItem
                //                                     {
                //                                         Item = y.Product.ProductName,
                //                                         TotalAmount = y.ItemPrice,
                //                                         UnitPrice = y.Product.UnitPrice,
                //                                         Quantity = (int)y.Quantity,
                //                                     }).ToList()
                //                                 }).ToArrayAsync();            
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return orders;
    }

    public async ValueTask<OrderDetails[]> GetOrderDetails()
    {
        OrderDetails[] details = Array.Empty<OrderDetails>();
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))
            {
                details = await _http.GetFromJsonAsync<OrderDetails[]>("api/orders/orderdetails") ?? Array.Empty<OrderDetails>();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // details = await db.ProductOrderDetails.AsSplitQuery()
                //                                       .Include(x => x.Product)
                //                                       .ToArrayAsync();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }
        return details;
    }

    public async Task<Order> GetOrder(Guid model)
    {
        Order order = new();
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))
            {
                order = await _http.GetFromJsonAsync<Order>($"api/orders/{model}") ?? new Order();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // order = await db.ProductOrder.FindAsync(model);
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }
        return order;
    }

    public async ValueTask<Order[]> GetOrders()
    {
        Order[] orders = Array.Empty<Order>();
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))                
            {
                orders = await _http.GetFromJsonAsync<Order[]>($"api/orders") ?? Array.Empty<Order>();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // orders = await db.ProductOrder.OrderByDescending(x => x.InvoiceNo).ToArrayAsync();                    
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return orders;
    }
        
    public async Task<int> GetReceiptNo()
    {
        int receiptNo = 0;
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))
            {
                receiptNo = await _http.GetFromJsonAsync<int>("api/orders/receiptno");
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // receiptNo = await db.ProductOrder.Select(i => i.InvoiceNo).CountAsync();
                // receiptNo += 1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return receiptNo;
    }

    public Task GetReport(string reportOption, DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public async Task GetReceipt(Guid Id)
    {
        byte[]? response = null;
        try
        {
            response = await _http.GetByteArrayAsync($"api/orders/getreceipt/{Id}");
        }
        catch
        {
            // await localDataStore.ProcessReceipt();
        }
        if (response != null)
            await _js.InvokeAsync<object>("exportExcelFile", $"Receipt-{Id}.html", Convert.ToBase64String(response));
    }

    public async Task<bool> UpdateOrder(Order model)
    {
        bool statusCode = false;
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))                
            {
                using var response = await _http.PutAsJsonAsync<Order>($"api/orders/{model.Id}", model);
                response.EnsureSuccessStatusCode();
                statusCode = response.IsSuccessStatusCode;
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // db.ProductOrder.Update(model);
                // var rowsAffected = await db.SaveChangesAsync();
                // if (rowsAffected >= 1)
                // {
                //     var local = await _localStorage.GetItemAsync<int>("localChanges");
                //     local++;
                //     await _localStorage.SetItemAsync<int>("localChanges", local);
                //     statusCode = true;
                // }                
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return statusCode;        
    }

    public async ValueTask GetReceipt(Order model, decimal NewPay)
    {     
        byte[]? response = null;  
        List<Receipt> receiptModel = new();
        var lastPayments = await GetPreviousPayment(model.Id);
        var lastPayment = lastPayments.FirstOrDefault();
        var orderDetails = model.OrderDetails.GroupBy(x => x.ProductID, (x, y) => new
        {
            Quantity = y.Sum(z => z.Quantity),
            ServiceName = y.Select(c => c.Product.ProductName).FirstOrDefault(),
            Cost = y.Sum(z => z.ItemPrice)
        }).ToList();
        foreach (var item in orderDetails)
        {
            var receiptItem = new Receipt();
            receiptItem.OrderID = model.Id;
            receiptItem.ReceiptNo = model.InvoiceNo.ToString().PadLeft(4, '0');
            receiptItem.CustomerName = model!.Customer!.CustomerName;
            receiptItem.OrderDate = model.TransactionDate.ToShortDateString();
            receiptItem.LastPayDate = lastPayment!.PaymentDate;
            receiptItem.NewPayDate = DateTime.Now;
            receiptItem.TotalAmount = model.TotalAmount;
            receiptItem.PreviousPayment = lastPayment.Amount;
            receiptItem.AmountPaid = NewPay;
            receiptItem.PreviousPayments = lastPayments.Sum(x => x.Amount);
            // receiptItem.Cashier = model.Employee.FullName;
            receiptItem.CatalogName = item.ServiceName;
            receiptItem.Cost = item.Cost;
            receiptItem.Discount = model.Discount;
            receiptItem.Quantity = item.Quantity;     
            receiptModel.Add(receiptItem);
        }
        string receiptNo = receiptModel.Select(x => x.ReceiptNo)!.FirstOrDefault()!;
        using var request = await _http.PostAsJsonAsync("api/orders/GetOrderReceipt", receiptModel);
        response = await request.Content.ReadAsByteArrayAsync();      
        await _js.InvokeAsync<object>("exportExcelFile", $"Receipt-{receiptNo}.html", Convert.ToBase64String(response));                        
    }

    public async ValueTask UpdatePayment(OrderPayment payment)
    {
        using var request = await _http.PostAsJsonAsync("api/orders/UpdatePayment", payment);
        request.EnsureSuccessStatusCode();            
    }

    public async ValueTask<List<OrderPayment>> GetPreviousPayment(Guid id)
    {
        return await _http.GetFromJsonAsync<List<OrderPayment>>($"api/orders/GetPreviousPayment/{id}") ?? new List<OrderPayment>();
    }

    public async ValueTask<CustomerWithOrder[]> GetCustomerWithOrders(Guid BranchID)
    {
        CustomerWithOrder[] orders = Array.Empty<CustomerWithOrder>();
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))
            {
                orders = await _http.GetFromJsonAsync<CustomerWithOrder[]>($"api/orders/BranchCustomerWithOrders/{BranchID}") ?? Array.Empty<CustomerWithOrder>();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // orders = await db.ProductOrder.AsSplitQuery()
                //                               .Include(c => c.Customer)
                //                               .Include(u => u.Employee)
                //                               .Include(x => x.ProductOrderDetails)
                //                               .ThenInclude(x => x.Product)
                //                               .OrderByDescending(i => i.InvoiceNo)
                //                               .Where(x => x.Employee.BranchID == BranchID)
                //                               .Select(e => new CustomerWithOrder()
                //                                 {
                //                                     OrderId = e.Id,
                //                                     Customer = e.Customer.CustomerName,
                //                                     PhoneNo = e.Customer.PhoneNo,
                //                                     Email = e.Customer.Email,
                //                                     ContactAddress = e.Customer.ContactAddress,
                //                                     OfficeAddress = e.Customer.OfficeAddress,
                //                                     ReceiptNo = e.InvoiceNo.ToString().PadLeft(4, '0'),
                //                                     OrderDate = e.TransactionDate,
                //                                     TotalAmount = e.TotalAmount,
                //                                     Discount = e.Discount,
                //                                     AmountPaid = e.AmountPaid,
                //                                     Vat = e.VAT,
                //                                     PaymentMode = e.PaymentMode,  
                //                                     OrderDetails = e.ProductOrderDetails.ToList().Select(y => new ReceiptItem
                //                                     {
                //                                         Item = y.Product.ProductName,
                //                                         TotalAmount = y.ItemPrice,
                //                                         UnitPrice = y.Product.UnitPrice,
                //                                         Quantity = (int)y.Quantity,
                //                                     }).ToList()
                //                                 }).ToArrayAsync();            
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return orders;
    }
}
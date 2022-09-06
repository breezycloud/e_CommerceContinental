
using Blazored.LocalStorage;
using e_CommerceContinental.Client.Services;
using e_CommerceContinental.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace e_CommerceContinental.Client.Pages.Orders;

public partial class BillingDialog
{    
    [CascadingParameter]
    public MudDialogInstance? DialogInstance { get; set; }
    public string PaymentMode { get; set; } = "Cash";
    bool isProcessing { get; set; } = false;
    Customer? customer =new();
    Customer[]? Customers = Array.Empty<Customer>();
    protected override async Task OnInitializedAsync()
    {        
        Customers = await CustomerService.GetCustomersOnly();
        
    }

    private async Task<IEnumerable<Customer>> Search(string value)
    {
        try
        {            
            if (string.IsNullOrEmpty(value))
            {
                return Customers!;
            }
            return await Task.FromResult(Customers!.Where(x => x.CustomerName!.Contains(value, StringComparison.InvariantCultureIgnoreCase)));
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
        }   
        return new HashSet<Customer>();     
    }

    private async Task CheckOut()
    {
        isProcessing= true;
        var prompt = await Dialog.ShowMessageBox("Confirmation", "Do you really wish to proceed!", yesText:"Yes", noText:"No");
        if (prompt is null || !prompt.Value)
        {
            isProcessing = false;
            return;
        }
        try
        {
            var userID = await _localStorage.GetItemAsync<string>("uid");    
            var parseUserID = Guid.Parse(userID);    
            Console.WriteLine($"User ID: {parseUserID}");
            var employee = await EmployeeService.GetEmployee(parseUserID);
            if (employee is not null)
            {
                AppState!.ReportDataModel!.Employee = new Employee();
                AppState!.ReportDataModel!.Employee = employee;
            }
            AppState!.ReportDataModel!.Customer = customer;
            var id = Guid.NewGuid();            
            var receiptNumber = await OrderService.GetReceiptNo();
            Order order = new()
            {
                Id = id,
                InvoiceNo = receiptNumber,
                TransactionDate = DateTime.Now,        
                CustomerID = customer!.CustomerId,
                TotalAmount = AppState!.Rows!.Sum(x=>x.Total), 
                AmountPaid = AppState!.Rows!.Sum(x=>x.Total),
                PaymentMode = PaymentMode,
                EmpID = parseUserID
            };
            AppState!.Rows!.ForEach((x) => 
            {
                order.OrderDetails.Add(new OrderDetails {
                    Id = Guid.NewGuid(),
                    OrderID = id,
                    ProductID = x.Product!.Id,
                    UnitPrice = x.Product.UnitPrice,
                    ItemPrice = x.Total,
                    Quantity = x.Quantity
                });              
            });
            // Save Order
            await OrderService.AddOrder(order);
            // Save Products
            await ProductService.EditProducts(AppState!.Products!.ToList());
            // Print receipt
            order.OrderDetails = new HashSet<OrderDetails>();
            AppState.ReportDataModel.Order = order;
            AppState!.ReportDataModel.PrevPage = "newsale";            
            nav.NavigateTo("receipt"); 
            AppState!.Rows.Clear();

        }
        catch (System.Exception ex)
        {            
            snackBar.Add("An error occured!", Severity.Error);
            Console.WriteLine(ex);
        }
        finally
        {
            isProcessing = false;
        }
    }

    private void CancelPayment() => DialogInstance!.Cancel();

}
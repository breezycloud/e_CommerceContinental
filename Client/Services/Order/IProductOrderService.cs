using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Services;

public interface IProductOrderService
{
    ValueTask<Order[]> GetOrders();
    ValueTask<OrderDetails[]> GetOrderDetails();
    ValueTask<CustomerWithOrder[]> GetCustomerWithOrders();
    ValueTask<CustomerWithOrder[]> GetCustomerWithOrders(Guid BranchID);
    Task<Order> GetOrder(Guid model);
    Task<bool> AddOrder(Order model);
    Task<bool> UpdateOrder(Order model);
    Task<bool> DeleteOrder(Order model);
    Task<int> GetReceiptNo();
    Task GetReceipt(Guid Id);
    ValueTask GetReceipt(Order receipt, decimal NewPay);
    Task GetReport(string reportOption, DateTime startDate, DateTime endDate);
    ValueTask<List<OrderPayment>> GetPreviousPayment(Guid id);
    ValueTask UpdatePayment(OrderPayment payment);
}
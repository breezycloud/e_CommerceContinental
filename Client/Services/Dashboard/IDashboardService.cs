using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Services;

interface IDashboardService
{
    ValueTask<int> GetTotalBranches();
    ValueTask<int> GetTotalShops();
    ValueTask<int> GetTotalEmployee();
    ValueTask<int> GetTotalCustomers();
    ValueTask<int> GetTotalProducts();
    ValueTask<int> GetTotalSales();
    ValueTask<Order[]> GetTopRecentProductsOrder();
    ValueTask<Dictionary<string, int>> GetTopProductPatronisedCustomer();    

    ValueTask<DashboardResponse> GetDashboardResponse();
    ValueTask<DashboardResponse> GetDashboardResponse(Guid id);
}

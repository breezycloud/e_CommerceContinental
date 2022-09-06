using Blazored.LocalStorage;
using e_CommerceContinental.Client.Data;
using e_CommerceContinental.Shared.Models;
using System.Net.Http.Json;

namespace e_CommerceContinental.Client.Services;

public class DashboardService : IDashboardService
{
    private readonly HttpClient _http;
    private readonly DbContextProvider _context;
    private ILocalStorageService _localStorage;

    public DashboardService(HttpClient http, DbContextProvider context, ILocalStorageService localStorage)
    {
        _http = http;
        _context = context;
        _localStorage = localStorage;
    }

    public async ValueTask<Dictionary<string, int>> GetTopProductPatronisedCustomer()
    {
        Dictionary<string, int> top = new();
        if (await _localStorage.GetItemAsync<bool>("mode"))
        {
            top = await _http.GetFromJsonAsync<Dictionary<string, int>>("api/dashboard/topcustomerproduct") ?? new Dictionary<string, int>();
        }
        else
        {
            await using var db = await _context.GetPreparedDbContextAsync();
        }
        return top;
    }        

    public async ValueTask<Order[]> GetTopRecentProductsOrder()
    {
        return await _http.GetFromJsonAsync<Order[]>("api/dashboard/toprecentproductorder") ?? Array.Empty<Order>();
    }
    

    public async ValueTask<int> GetTotalBranches()
    {
        int TotalBranches = 0;
        if (await _localStorage.GetItemAsync<bool>("mode"))
        {
            TotalBranches = await _http.GetFromJsonAsync<int>("api/dashboard/totalbranch");
        }
        else
        {
            await using var db = await _context.GetPreparedDbContextAsync();
            TotalBranches = db.Branches.Count();
        }
        return TotalBranches;
    }

    public async ValueTask<int> GetTotalShops()
    {
        int TotalBranches = 0;
        if (await _localStorage.GetItemAsync<bool>("mode"))
        {
            TotalBranches = await _http.GetFromJsonAsync<int>("api/dashboard/totalshops");
        }
        else
        {
            await using var db = await _context.GetPreparedDbContextAsync();
            TotalBranches = db.Branches.Count();
        }
        return TotalBranches;
    }

    public async ValueTask<int> GetTotalSales()
    {
        int TotalBranches = 0;
        if (await _localStorage.GetItemAsync<bool>("mode"))
        {
            TotalBranches = await _http.GetFromJsonAsync<int>("api/dashboard/totalsales");
        }
        else
        {
            await using var db = await _context.GetPreparedDbContextAsync();
            TotalBranches = db.Orders.Count();
        }
        return TotalBranches;
    }

    public async ValueTask<int> GetTotalCustomers()
    {
        int TotalCustomers = 0;
        if (await _localStorage.GetItemAsync<bool>("mode"))
        {
            TotalCustomers = await _http.GetFromJsonAsync<int>("api/dashboard/totalcustomers");
        }
        else
        {
            await using var db = await _context.GetPreparedDbContextAsync();
            TotalCustomers = db.Customers.Count();
        }
        return TotalCustomers;
    }

    public async ValueTask<int> GetTotalEmployee()
    {
        int totalEmployees = 0;
        if (await _localStorage.GetItemAsync<bool>("mode"))
            totalEmployees = await _http.GetFromJsonAsync<int>("api/dashboard/totalemployees");
        else
        {
            await using var db = await _context.GetPreparedDbContextAsync();
            totalEmployees = db.Employees.Where(x => x.PhoneNo != "07068716813").Count();
        }
        return totalEmployees;
    }

    public async ValueTask<int> GetTotalProducts()
    {
        int TotalProducts = 0;
        if (await _localStorage.GetItemAsync<bool>("mode"))
        {
            TotalProducts = await _http.GetFromJsonAsync<int>("api/dashboard/totalproducts");
        }        
        else
        {
            await using var db = await _context.GetPreparedDbContextAsync();            
            TotalProducts = db.Products.Where(x => x.StocksOnHand >= 1).Count();
        }
        return TotalProducts;
    }

    public async ValueTask<DashboardResponse> GetDashboardResponse()
    {
        return await _http.GetFromJsonAsync<DashboardResponse>("api/dashboard/getdata") ?? new DashboardResponse();
    }

    public ValueTask<DashboardResponse> GetDashboardResponse(Guid id)
    {
        throw new NotImplementedException();
    }
}

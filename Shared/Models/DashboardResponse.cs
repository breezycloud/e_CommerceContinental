namespace e_CommerceContinental.Shared.Models;

public class DashboardResponse
{
    public TopChartData? TopChartData { get; set; }
    public OrderLineChart[]? OrdersLineChart { get; set; }
    public BranchSales[]? BranchSales { get; set; }
    public BranchShopSales[]? BranchShopSales { get; set; }
    public ProductsPieChart[]? ProductsPieCharts { get; set; }
}

public class BranchSales
{
    public string? BranchName { get; set; }
    public int TotalSales { get; set; }
}

public class BranchShopSales
{
    public string? BranchName { get; set; }
    public string? ShopName { get; set; }
    public int TotalSales { get; set; }
}


public class OrderLineChart
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int TotalSales { get; set; }
}

public class ProductsPieChart
{
    public string? Product { get; set; }
    public double? TotalSales { get; set; }
}

public class TopChartData
{
    public int TotalEmployees { get; set; }
    public int TotalBranches { get; set; }
    public int TotalShops { get; set; }
    public int TotalProducts { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalSales { get; set; }
}
using MudBlazor.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using e_CommerceContinental.Client.Handlers;
using e_CommerceContinental.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Diagnostics.CodeAnalysis;
using e_CommerceContinental.Client;
using e_CommerceContinental.Client.Data;
using Microsoft.EntityFrameworkCore;
using e_CommerceContinental.Shared.Models;

public class Program
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    // ReSharper disable once UnusedMember.Local
    #pragma warning disable IDE0052
        private static readonly Type s_keepDateOnly = typeof(DateOnly);
    #pragma warning restore IDE0052

    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddOptions();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddMudServices();
        builder.Services.AddBlazoredLocalStorage();

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });        

        builder.Services.AddHttpClient<CustomAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(options => options.GetRequiredService<CustomAuthenticationStateProvider>());

        builder.Services.AddTransient<CustomAuthorizationHandler>()
                        .AddDbContextFactory<AppDbContext>(options => options.UseSqlite($"Filename=app.db"))
                        .AddScoped<DbContextProvider>()
                        .AddScoped<Synchronizer>();
        builder.Services.AddScoped<LayoutService>();
        builder.Services.AddScoped<BackgroundTask>().AddScoped<ILocalDataFactory, LocalDataFactory>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IUserPreferencesService, UserPreferencesService>();

        builder.Services.AddHttpClient<IUserService, UserService>("UserService", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        }).AddHttpMessageHandler<CustomAuthorizationHandler>();

        builder.Services.AddHttpClient<IEmployeeService, EmployeeService>("EmpService", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        }).AddHttpMessageHandler<CustomAuthorizationHandler>();

        builder.Services.AddHttpClient<IBankService, BankService>("BankService", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        }).AddHttpMessageHandler<CustomAuthorizationHandler>();

        builder.Services.AddHttpClient<IDashboardService, DashboardService>("Dashboard", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        }).AddHttpMessageHandler<CustomAuthorizationHandler>();

        builder.Services.AddHttpClient<ICustomerService, CustomerService>("Customer", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        }).AddHttpMessageHandler<CustomAuthorizationHandler>();

        builder.Services.AddHttpClient<IProductOrderService, ProductOrderService>("OrderService", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        }).AddHttpMessageHandler<CustomAuthorizationHandler>();

        builder.Services.AddHttpClient<IBranchService, BranchService>("BranchService", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        }).AddHttpMessageHandler<CustomAuthorizationHandler>();

        builder.Services.AddHttpClient<IProductService, ProductService>("ProductService", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        }).AddHttpMessageHandler<CustomAuthorizationHandler>();

        builder.Services.AddHttpClient<IShopService, ShopService>("ShopService", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        }).AddHttpMessageHandler<CustomAuthorizationHandler>();
        
        builder.Services.AddHttpClient<IRoleService, RoleService>("RoleService", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        }).AddHttpMessageHandler<CustomAuthorizationHandler>();

        builder.Services.AddHttpClient<IMenuService, MenuService>("MenuService", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        }).AddHttpMessageHandler<CustomAuthorizationHandler>();

        builder.Services.AddScoped<AppState>();

        await builder.Build().RunAsync();
        
    }
}

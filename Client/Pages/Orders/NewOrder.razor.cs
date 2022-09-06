
using Blazored.LocalStorage;
using e_CommerceContinental.Client.Services;
using e_CommerceContinental.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace e_CommerceContinental.Client.Pages.Orders;

public partial class NewOrder
{
    [CascadingParameter]
    public Task<AuthenticationState>? AuthenticationState { get; set; }     
    MudAutocomplete<Product>? _searchProducts;
    Product? product = new();
    Category[]? Categories = Array.Empty<Category>();
    CartRow? Row { get; set; } = default!;
    Guid ShopID;

    protected override async Task OnInitializedAsync()
    {
        var userAuthenticationState = await AuthenticationState!;
        var isAuthenticated = userAuthenticationState.User;
        if (isAuthenticated.IsInRole("Master"))
        {
            AppState!.Products = await ProductService.GetProductsOnly();
            Categories = AppState!.Products.Any() ? AppState!.Products.Select(x => x.Category!).Distinct().ToArray() : Array.Empty<Category>();
        }            
        else
        {
            ShopID = await _localStorage.GetItemAsync<Guid>("shop");
            AppState!.Products = await ProductService.GetProductsByBranch(ShopID);
            Categories = AppState!.Products.Any() ? AppState!.Products.Select(x => x.Category!).Distinct().ToArray() : Array.Empty<Category>();
        }     
        AppState.OnUpdate += StateHasChanged;   
    }

    private async Task<IEnumerable<Product>> Search(string text)
    {
        if (string.IsNullOrEmpty(text))
            return await Task.FromResult(AppState!.Products!.Take(20));
        
        return await Task.FromResult(
            AppState!.Products!.Where(x => x.ProductName!.Contains(text, StringComparison.OrdinalIgnoreCase) || 
            x.Category!.CategoryName!.Contains(text, StringComparison.OrdinalIgnoreCase))
        );
    }

    private async Task SearchByBarcode(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return;
        
        await Task.Delay(100);
        Row!.Product = AppState!.Products!.Where(x => x.Barcode == text).FirstOrDefault() ?? new Product();
        StateHasChanged();
    }

    private async void OnSearchResult(Product product)
    {        
        await OnCart(product);
    }

    private async Task OnCart(Product product)
    {
        if (AppState!.Products!.Where(x => x.Id == product.Id).First().StocksOnHand < 1)
        {
            snackBar.Add("Item is out of stock", Severity.Warning);
            return;
        }
        if (AppState.Rows!.Any(x => x.Product!.Id ==product.Id))
        {
            return;
        }
        else
        {
            AppState.Rows!.Add(new CartRow { Product = product, Quantity = 1 });    
            AppState!.Products!.Where(x => x.Id == product.Id).FirstOrDefault()!.StocksOnHand -= 1;
        }
        await AppState!.OnNotify();
    }    

    private void OpenCheckOut()
    {
        if (!AppState!.Rows!.Any())
        {
            snackBar.Add("Cart is empty, add items to proceed!", Severity.Warning);
            return;
        }
        Dialog.Show<BillingDialog>("Billing Form", new DialogOptions { CloseButton = true, CloseOnEscapeKey = true});
    }
 }
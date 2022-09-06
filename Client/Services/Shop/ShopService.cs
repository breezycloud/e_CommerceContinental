using System.Net.Http.Json;
using Blazored.LocalStorage;
using e_CommerceContinental.Client.Data;
using e_CommerceContinental.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace e_CommerceContinental.Client.Services;

public class ShopService : IShopService
{
    private readonly HttpClient _http =default!;
    private readonly DbContextProvider _context;
    private ILocalStorageService _localStorage;
    private Shop Shop { get; set; } = default!;
    private Shop[] Shops { get; set; } = default!;

    public ShopService(HttpClient http, DbContextProvider context, ILocalStorageService localStorage)
    {
        _http = http;
        _context = context;
        _localStorage = localStorage;
    }

    public async ValueTask<bool> AddShop(Shop shop)
    {
        if (await _localStorage.GetItemAsync<bool>("mode"))                  
        {
            using var request = await _http.PostAsJsonAsync("api/shops", shop);
            request.EnsureSuccessStatusCode();
            return request.IsSuccessStatusCode;
        }            
        else
        {
            using var db = await _context.GetPreparedDbContextAsync();
            await db.Shops.AddAsync(shop);
            int rowsAffected = await db.SaveChangesAsync();
            if (rowsAffected > 1)
                return true;                
        }
        return false;
    }

    public async ValueTask<bool> DeleteShop(Guid id)
    {
        if (await _localStorage.GetItemAsync<bool>("mode"))                  
        {
            using var request = await _http.DeleteAsync($"api/shops/{id}");
            request.EnsureSuccessStatusCode();
            return request.IsSuccessStatusCode;
        }            
        else
        {
            // using var db = await _context.GetPreparedDbContextAsync();
            // await db.Shops.AddAsync(shop);
            // int rowsAffected = await db.SaveChangesAsync();
            // if (rowsAffected > 1)
                return true;                
        }
        // return false;
    }

    public async ValueTask<bool> EditShop(Shop shop)
    {
        if (await _localStorage.GetItemAsync<bool>("mode"))                  
        {
            using var request = await _http.PutAsJsonAsync($"api/shops/{shop.Id}", shop);
            request.EnsureSuccessStatusCode();
            return request.IsSuccessStatusCode;
        }            
        else
        {
            using var db = await _context.GetPreparedDbContextAsync();
            db.Shops.Update(shop);
            int rowsAffected = await db.SaveChangesAsync();
            if (rowsAffected > 1)
                return true;                
        }
        return false;
    }

    public async ValueTask<Shop> GetShop(Guid id)
    {
        if (await _localStorage.GetItemAsync<bool>("mode"))                  
        {
            Shop = await _http.GetFromJsonAsync<Shop>($"api/shops/{id}") ?? new Shop();            
        }            
        else
        {
            using var db = await _context.GetPreparedDbContextAsync();
            Shop = await db.Shops.FindAsync(id) ?? new Shop();
        }
        return Shop;
    }

    public async ValueTask<Shop[]> GetShops()
    {
        if (await _localStorage.GetItemAsync<bool>("mode"))                  
        {
            Shops = await _http.GetFromJsonAsync<Shop[]>($"api/shops") ?? Array.Empty<Shop>();
        }            
        else
        {
            using var db = await _context.GetPreparedDbContextAsync();
            Shops = await db.Shops.ToArrayAsync() ?? Array.Empty<Shop>();
        }
        return Shops;
    }

    public async ValueTask<Shop[]> GetShopsByBranch(Guid BranchID)
    {
        if (await _localStorage.GetItemAsync<bool>("mode"))                  
        {
            Shops = await _http.GetFromJsonAsync<Shop[]>($"api/shops/byBranch/{BranchID}") ?? Array.Empty<Shop>();
        }            
        else
        {
            using var db = await _context.GetPreparedDbContextAsync();
            Shops = await db.Shops.Where(x => x.BranchID == BranchID).ToArrayAsync() ?? Array.Empty<Shop>();
        }
        return Shops;
    }
}
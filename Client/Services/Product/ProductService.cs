using System.Net.Http.Json;
using Blazored.LocalStorage;
using e_CommerceContinental.Client.Data;
using e_CommerceContinental.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace e_CommerceContinental.Client.Services;

class ProductService : IProductService
{
    private readonly HttpClient _http;
    private readonly DbContextProvider? _context;
    private ILocalStorageService _localStorage;
    private bool responseResult { get; set; }
    private Category[] categories = Array.Empty<Category>();
    private Category Category { get; set; } = default!;
    private Product[] products = Array.Empty<Product>();
    private Product Product { get; set; } = default!;

    public ProductService(HttpClient http, DbContextProvider context, ILocalStorageService localStorage)
    {
        _http = http;
        _context = context;
        _localStorage = localStorage;
    }

    public async ValueTask<bool> AddProduct(Product product)
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                using var response = await _http.PostAsJsonAsync("api/products", product);
                response.EnsureSuccessStatusCode();
                responseResult = response.IsSuccessStatusCode;
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // await db.Product.AddAsync(product);
                // int rowsAffected = await db.SaveChangesAsync();
                // if (rowsAffected >= 1)
                // {
                //     var local = await _localStorage.GetItemAsync<int>("localChanges");
                //     local++;
                //     await _localStorage.SetItemAsync<int>("localChanges", local);
                //     responseResult = true;                
                // }                    
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return responseResult;
    }    

    public async ValueTask<bool> DeleteProduct(Guid id)
    {
        using var response = await _http.DeleteAsync($"api/products/{id}");
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async ValueTask<bool> EditProduct(Guid id, Product product)
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                using var response = await _http.PutAsJsonAsync($"api/products/{id}", product);
                response.EnsureSuccessStatusCode();
                responseResult = response.IsSuccessStatusCode;
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // db.Product.Update(product);
                // int rowsAffected = await db.SaveChangesAsync();
                // if (rowsAffected >= 1)
                // {
                //     var local = await _localStorage.GetItemAsync<int>("localChanges");
                //     local++;
                //     await _localStorage.SetItemAsync<int>("localChanges", local);
                //     responseResult = true;                
                // }                    
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return responseResult;
        
    }
    public async ValueTask<bool> EditProducts(List<Product> products)
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                using var response = await _http.PutAsJsonAsync($"api/products/many/", products);
                response.EnsureSuccessStatusCode();
                responseResult = response.IsSuccessStatusCode;
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // db.Product.Update(product);
                // int rowsAffected = await db.SaveChangesAsync();
                // if (rowsAffected >= 1)
                // {
                //     var local = await _localStorage.GetItemAsync<int>("localChanges");
                //     local++;
                //     await _localStorage.SetItemAsync<int>("localChanges", local);
                //     responseResult = true;                
                // }                    
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return responseResult;
    }

    public async ValueTask<Product[]> GetProducts()
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                products = await _http.GetFromJsonAsync<Product[]>("api/products/related") ?? Array.Empty<Product>();  
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // products = await db.Product.AsSplitQuery()
                //                            .Include(x => x.Branch)
                //                            .Include(x => x.Category)
                //                            .ToArrayAsync();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return products;        
    }

    public async ValueTask<Product[]> GetProductsOnly()
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                products = await _http.GetFromJsonAsync<Product[]>("api/products") ?? Array.Empty<Product>();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // products = await db.Product.ToArrayAsync();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return products;        
    }
    public async ValueTask<Product> GetProduct(Guid id)
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                Product = await _http.GetFromJsonAsync<Product>($"api/products/{id}") ?? new Product();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // Product = await db.Product.FindAsync(id);
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }
        return Product;
    }

    public async ValueTask<Product[]> GetProductsByBranch(Guid id)
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                products = await _http.GetFromJsonAsync<Product[]>($"api/products/byBranch/{id}") ?? Array.Empty<Product>();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // products = await db.Product.Where(x => x.BranchID == id && x.StocksOnHand >= 1)
                //                            .ToArrayAsync();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return products;        
    }

    public async ValueTask<Product[]> GetProductsByShop(Guid id)
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                products = await _http.GetFromJsonAsync<Product[]>($"api/products/byShop/{id}") ?? Array.Empty<Product>();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // products = await db.Product.Where(x => x.ShopID == id && x.StocksOnHand >= 1)
                //                            .ToArrayAsync();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return products;        
    }

    public async ValueTask<Category[]> GetCategories()
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                categories = await _http.GetFromJsonAsync<Category[]>($"api/Category") ?? Array.Empty<Category>();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // categories = await db.Category.OrderBy(x => x.CategoryName).ToArrayAsync();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return categories;
    }

    public async ValueTask<Category> GetCategory(Guid id)
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                Category = await _http.GetFromJsonAsync<Category>($"api/Category/{id}") ?? new Category();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // Category = await db.Category.FindAsync(id);
            }            
        }
        catch (System.Exception)
        {
            
            throw;
        }
        return Category;
    }

    public async ValueTask<Product[]> GetProductsByCategory(Guid id)
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                products =await _http.GetFromJsonAsync<Product[]>($"api/products/bycategory/{id}") ?? Array.Empty<Product>();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // products = await db.Product.Where(x => x.CategoryID == id && x.StocksOnHand >= 1)
                //                            .ToArrayAsync();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return products;                
    }

    public async ValueTask<bool> AddCategory(Category category)
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                using var response = await _http.PostAsJsonAsync("api/Category", category);
                response.EnsureSuccessStatusCode();
                responseResult = response.IsSuccessStatusCode;
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // await db.Category.AddAsync(category);
                // int rowsAffected = await db.SaveChangesAsync();
                // if (rowsAffected >= 1)
                // {
                //     var local = await _localStorage.GetItemAsync<int>("localChanges");
                //     local++;
                //     await _localStorage.SetItemAsync<int>("localChanges", local);
                //     responseResult = true;                
                // }                    
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return responseResult;
    }

    public async ValueTask<bool> EditCategory(Guid id, Category category)
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                using var response = await _http.PutAsJsonAsync($"api/Category/{id}", category);
                response.EnsureSuccessStatusCode();
                responseResult = response.IsSuccessStatusCode;
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // db.Category.Update(category);
                // int rowsAffected = await db.SaveChangesAsync();
                // if (rowsAffected >= 1)
                // {
                //     var local = await _localStorage.GetItemAsync<int>("localChanges");
                //     local++;
                //     await _localStorage.SetItemAsync<int>("localChanges", local);
                //     responseResult = true;                
                // }                    
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return responseResult;
    }

    public async ValueTask<bool> DeleteCategory(Guid id)
    {
        using var response = await _http.DeleteAsync($"api/Category/{id}");
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async ValueTask<bool> AddStock(Stock stock)
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                using var response = await _http.PostAsJsonAsync("api/stocks", stock);
                response.EnsureSuccessStatusCode();
                responseResult = response.IsSuccessStatusCode;
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // await db.Stock.AddAsync(stock);
                // int rowsAffected = await db.SaveChangesAsync();
                // if (rowsAffected >= 1)
                // {
                //     var local = await _localStorage.GetItemAsync<int>("localChanges");
                //     local++;
                //     await _localStorage.SetItemAsync<int>("localChanges", local);
                //     responseResult = true;                
                // }                    
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return responseResult;            
    }

    public async ValueTask<bool> EditStock(Guid id, Stock stock)
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                using var response = await _http.PutAsJsonAsync($"api/stocks/{id}", stock);
                response.EnsureSuccessStatusCode();
                responseResult = response.IsSuccessStatusCode;
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // db.Stock.Update(stock);
                // int rowsAffected = await db.SaveChangesAsync();
                // if (rowsAffected >= 1)
                // {
                //     var local = await _localStorage.GetItemAsync<int>("localChanges");
                //     local++;
                //     await _localStorage.SetItemAsync<int>("localChanges", local);
                //     responseResult = true;                
                // }                    
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }        
        return responseResult;                            
    }

    public async ValueTask<bool> DeleteStock(Guid id)
    {
        using var response = await _http.DeleteAsync($"api/stocks/{id}");
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async ValueTask<Product> ExistProductCode(string code)
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))
            {
                Product = await _http.GetFromJsonAsync<Product>($"api/products/existproductcode/{code}") ?? new Product();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // Product = await db.Product.Where(x => x.Code == code).FirstOrDefaultAsync() ?? new Product();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }
        return Product;
    }

    public async ValueTask<Product[]> GetFabricProducts()
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))
            {
                await _http.GetFromJsonAsync<Product[]>("api/products/fabrics");
            }
            else            
            {
                // await using var db = await _context!.GetPreparedDbContextAsync();
                // products = await db.Product.AsSplitQuery().Include(x => x.Category).Where(x => x.Category.CategoryName == "Fabrics").Select(n => new Product{
                //     Id = n.Id,
                //     ProductName = n.ProductName,
                //     Description = n.Description,
                //     CategoryID = n.CategoryID,
                //     Barcode = n.Barcode,
                // }).ToArrayAsync();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }
        return products;
    }        
}
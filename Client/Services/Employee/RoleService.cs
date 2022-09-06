using System.Net.Http.Json;
using Blazored.LocalStorage;
using e_CommerceContinental.Client.Data;
using e_CommerceContinental.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace e_CommerceContinental.Client.Services;

public class RoleService : IRoleService
{
    private readonly HttpClient _http;
    private readonly DbContextProvider _context;
    private ILocalStorageService _localStorage;
    private bool responseResult { get; set; }
    public RoleService(HttpClient http, DbContextProvider context, ILocalStorageService localStorage)
    {
        _http = http;
        _context = context;
        _localStorage = localStorage;
    }

    public async ValueTask<bool> AddRole(Role role)
    {

        if (await _localStorage.GetItemAsync<bool>("mode"))
        {
            using var response = await _http.PostAsJsonAsync("api/roles", role);
            response.EnsureSuccessStatusCode();
            responseResult = response.IsSuccessStatusCode;
        }        
        else
        {
            await using var db = await _context.GetPreparedDbContextAsync();
            // await db.Roles.AddAsync(role);
            // int rowsAffected = await db.SaveChangesAsync();
            // if (rowsAffected >= 1)
            // {
            //     var local = await _localStorage.GetItemAsync<int>("localChanges");
            //     local++;
            //     await _localStorage.SetItemAsync<int>("localChanges", local);
            //     responseResult = true;                
            // }
        }
        return responseResult;
    }

    public async ValueTask<bool> DeleteRole(Guid id)
    {
        using var response = await _http.DeleteAsync($"api/roles/{id}");
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async ValueTask<bool> EditRole(Guid id, Role role)
    {
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))
            {
                using var response = await _http.PutAsJsonAsync($"api/roles/{id}", role);
                response.EnsureSuccessStatusCode();
                responseResult = response.IsSuccessStatusCode;
            } 
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // db.Roles.Update(role);
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

    public async ValueTask<Role> GetRole(Guid id)
    {
        Role role = new();
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))            
            {
                role = await _http.GetFromJsonAsync<Role>($"api/roles/{id}") ?? new Role();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // role = await db.Roles.FindAsync(id);
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }
        return role;
    }

    public async ValueTask<Role[]> GetRolesExcludingMaster()
    {
        Role[] roles = Array.Empty<Role>();
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))
            {
                roles = await _http.GetFromJsonAsync<Role[]>($"api/roles/excludingmaster") ?? Array.Empty<Role>();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // roles = await db.Roles.Where(x => x.RoleType != "Master").ToArrayAsync();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);       
        }
        return roles;
    }
    public async ValueTask<Role[]> GetRoles()
    {
        Role[] roles = Array.Empty<Role>();
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))
            {
                roles = await _http.GetFromJsonAsync<Role[]>($"api/roles") ?? Array.Empty<Role>();
            }
            else
            {
                // await using var db = await _context.GetPreparedDbContextAsync();
                // roles = await db.Roles.ToArrayAsync();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);       
        }
        return roles;
    }
}
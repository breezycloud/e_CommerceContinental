using System.Net.Http.Json;
using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Services;

public interface IMenuService
{
    ValueTask<Menu[]> GetMenus();
    ValueTask<Menu> GetMenu(Guid id);
    ValueTask<bool> AddMenu(Menu menu);
    ValueTask<bool> AddSubMenu(SubMenu menu);
    ValueTask<bool> EditMenu(Menu menu);
    ValueTask<bool> EditSubMenu(SubMenu menu);
    ValueTask<bool> DeleteMenu(Guid id);
    ValueTask<bool> DeleteSubMenu(Guid id);
}


public class MenuService : IMenuService
{
    private readonly HttpClient _http;

    public MenuService(HttpClient http)
    {
        _http = http;
    }

    public async ValueTask<bool> AddMenu(Menu menu)
    {
        using var request = await _http.PostAsJsonAsync("api/menus", menu);
        request.EnsureSuccessStatusCode();
        return request.IsSuccessStatusCode;
    }

    public async ValueTask<bool> AddSubMenu(SubMenu menu)
    {
        using var request = await _http.PostAsJsonAsync("api/menus/submenu", menu);
        request.EnsureSuccessStatusCode();
        return request.IsSuccessStatusCode;
    }

    public async ValueTask<bool> DeleteMenu(Guid id)
    {
        using var request = await _http.DeleteAsync($"api/menus/{id}");
        request.EnsureSuccessStatusCode();
        return request.IsSuccessStatusCode;
    }

    public async ValueTask<bool> DeleteSubMenu(Guid id)
    {
        using var request = await _http.DeleteAsync($"api/menus/submenu/{id}");
        request.EnsureSuccessStatusCode();
        return request.IsSuccessStatusCode;
    }

    public async ValueTask<bool> EditMenu(Menu menu)
    {
        using var request = await _http.PutAsJsonAsync($"api/menus/{menu.Id}", menu);
        request.EnsureSuccessStatusCode();
        return request.IsSuccessStatusCode;
    }

    public async ValueTask<bool> EditSubMenu(SubMenu menu)
    {
        using var request = await _http.PutAsJsonAsync($"api/menus/submenu/{menu.Id}", menu);
        request.EnsureSuccessStatusCode();
        return request.IsSuccessStatusCode;
    }

    public async ValueTask<Menu> GetMenu(Guid id)
    {
        return await _http.GetFromJsonAsync<Menu>($"api/menus/{id}") ?? new Menu();
    }

    public async ValueTask<Menu[]> GetMenus()
    {
        return await _http.GetFromJsonAsync<Menu[]>($"api/menus") ?? Array.Empty<Menu>();
    }
}
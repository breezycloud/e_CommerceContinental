using Blazored.LocalStorage;
using e_CommerceContinental.Shared.Models;
using System.Net.Http.Json;

namespace e_CommerceContinental.Client.Services;

public class BranchService : IBranchService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    public BranchService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    public async ValueTask<Guid> GetBranchID() => await _localStorage.GetItemAsync<Guid>("branch");

    public async ValueTask<Branch[]> GetBranches()
    {
        return await _http.GetFromJsonAsync<Branch[]>("api/Branches") ?? Array.Empty<Branch>();
    }
    public async ValueTask<Branch> GetBranch(Guid id)
    {
        return await _http.GetFromJsonAsync<Branch>($"api/Branches/{id}") ?? new Branch();
    }
    public async ValueTask<bool> AddBranch(Branch branch)
    {
        using var response = await _http.PostAsJsonAsync("api/Branches", branch);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }
    public async ValueTask<bool> EditBranch(Guid id, Branch branch)
    {
        using var response = await _http.PutAsJsonAsync($"api/Branches/{id}", branch);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }
    public async ValueTask<bool> DeleteBranch(Guid id)
    {
        using var response = await _http.DeleteAsync($"api/Branches/{id}");
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }
}
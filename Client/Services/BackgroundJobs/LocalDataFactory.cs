using System.Net.Http.Json;
using Blazored.LocalStorage;
using e_CommerceContinental.Shared;
using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Services;

public interface ILocalDataFactory
{
    ValueTask<bool> ConnectionStatus();
    ValueTask DataCounter();
    ValueTask<int> TotalChanges();
    ValueTask<SynchronizationModel> DownloadChanges(long ticks, Guid id);
}

public class LocalDataFactory : ILocalDataFactory
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;

    public LocalDataFactory(ILocalStorageService localStorage, HttpClient http)
    {
        _localStorage = localStorage;
        _http = http;        
    }

    public async ValueTask<bool> ConnectionStatus() => await _localStorage.GetItemAsync<bool>("mode");
    
    public async ValueTask DataCounter()
    {
        int currentValue = await _localStorage.GetItemAsync<int>("localChanges");
        currentValue++;
        await _localStorage.SetItemAsync("localChanges", currentValue);
    }

    public async ValueTask<int> TotalChanges() => await _localStorage.GetItemAsync<int>("localChanges");
    public async ValueTask<SynchronizationModel> DownloadChanges(long ticks, Guid id)
    {
        return await ValueTask.FromResult(new SynchronizationModel());
    }
}
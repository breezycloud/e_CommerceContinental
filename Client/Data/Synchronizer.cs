using System.Net.Http.Json;
using Blazored.LocalStorage;
using e_CommerceContinental.Client.Services;
using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Data;

public class Synchronizer
{
    private readonly DbContextProvider _dbContextProvider = default!;
    private readonly ILocalStorageService _localStorage  = default!;
    private readonly HttpClient _httpClient  = default!;
    public bool isDownloading { get; set; } = default!;
    public bool isUploading { get; set; }  = default!;
    public string SyncLabel { get; private set; }  = default!;
    public int SyncCompleted { get; private set; }  = default!;
    public int SyncTotal { get; private set; }  = default!;
    private HttpResponseMessage request {get; set; } = default!;
    private readonly ILocalDataFactory _sync  = default!;

    public Synchronizer(HttpClient httpClient,
        DbContextProvider dbContextProvider,
        ILocalStorageService localStorage,
        ILocalDataFactory sync)
    {
        _httpClient = httpClient;
        _dbContextProvider = dbContextProvider;
        _localStorage = localStorage;
        _sync = sync;
    }

    public void SynchronizeInBackground(string mode)
    {        
        // if (mode == "Upload")
        //     _ = UploadSynchronizeAsync();
        // else
        //     _ = SynchronizeAsync();
    }

    public event Action? OnUpdate;
    public event Action<Exception>? OnError;    
}
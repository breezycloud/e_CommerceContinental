using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.JSInterop;
using System.Text.Json;
using Blazored.LocalStorage;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using e_CommerceContinental.Shared.Models;

#nullable disable
namespace e_CommerceContinental.Client;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private ILocalStorageService _localStorage;
    private HttpClient _httpClient;
    private NavigationManager _navigation;
    public CustomAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient, NavigationManager navigation)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
        _navigation = navigation;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var claims = ParseClaimsFromJwt(token);
        var expiry = claims.Where(claim => claim.Type.Equals("exp")).FirstOrDefault();
        if (expiry == null)
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        // The exp field is in Unix time
        var datetime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry.Value));
        if (datetime.UtcDateTime <= DateTime.UtcNow)
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        //var identity = string.IsNullOrEmpty(token)
        //    ? new ClaimsIdentity()
        //    : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
    }

    public async Task SetTokenAsync(string branchID, string token, string userID)
    {
        if (token == null)
        {
            await _localStorage.RemoveItemAsync("token");
            await _localStorage.RemoveItemAsync("uid");
            await _localStorage.RemoveItemAsync("branch");
        }
        else
        {
            await _localStorage.SetItemAsync("token", token);
            await _localStorage.SetItemAsync("uid", userID);
            await _localStorage.SetItemAsync("branch", branchID);
        }

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task SetTokenAsync(LoginModel loginModel)
    {
        if (loginModel is null)
        {
            await _localStorage.RemoveItemAsync("role");
            await _localStorage.RemoveItemAsync("token");
            await _localStorage.RemoveItemAsync("uid");
            await _localStorage.RemoveItemAsync("branch");
            if (loginModel.AccessPrivilege != "Master" || loginModel.AccessPrivilege != "Admin")
                await _localStorage.RemoveItemAsync("shop");
        }
        else
        {
            await _localStorage.SetItemAsync("role", loginModel.AccessPrivilege);
            await _localStorage.SetItemAsync("token", loginModel.Token);
            await _localStorage.SetItemAsync("uid", loginModel.Id);
            await _localStorage.SetItemAsync("branch", loginModel.BranchID);
            if (loginModel.AccessPrivilege != "Master" || loginModel.AccessPrivilege != "Admin")
                await _localStorage.SetItemAsync("shop", loginModel.ShopID);
        }

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<string> GetTokenAsync()
        => await _localStorage.GetItemAsync<string>("token");

    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    public void LogOutNotfiy()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}

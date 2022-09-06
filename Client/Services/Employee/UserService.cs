using System.Net.Http.Headers;
using e_CommerceContinental.Shared.Models;
using e_CommerceContinental.Client.Data;
using Blazored.LocalStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json;
using e_CommerceContinental.Client.Util;

#nullable disable
namespace e_CommerceContinental.Client.Services;

public class UserService : IUserService
{
    public HttpClient _httpClient { get; }
    private readonly DbContextProvider _context;
    private ILocalStorageService _localStorage;
    private readonly IJSRuntime _js;
    public UserService(HttpClient httpClient, DbContextProvider context, ILocalStorageService localStorage, IJSRuntime js)
    {
        _httpClient = httpClient;
        _context = context;
        _localStorage = localStorage;
        _js = js;
    }

    public async Task<LoginModel> Login(LoginModel model)
    {
        LoginModel result = new();
        try
        {
            if (await _localStorage.GetItemAsync<bool>("mode"))
            {                

                using var response = await _httpClient.PostAsJsonAsync("api/users/login", model);
                result = await response.Content.ReadFromJsonAsync<LoginModel>();
            }
            else
            {
                await using var db = await _context.GetPreparedDbContextAsync();
                string hashedPassword = Security.Encrypt(model.Password);
                // result = await db.EmployeeAccounts.AsSplitQuery()
                //                                   .Include(x => x.Employee)
                //                                   .Include(r => r.Role)
                //                                   .Where(u => u.Username == model.Username && u.HashedPassword == hashedPassword&& u.IsActive == true)
                //                                   .Select(x => new LoginModel {
                //                                         Id = x.EmpID,
                //                                         BranchID = x.Employee.BranchID,
                //                                         Token = "",
                //                                         AccessPrivilege = x.Role.RoleType
                //                                     }).FirstOrDefaultAsync();
                
                // var jwt = await _js.InvokeAsync<string>("authenticate", result);                                        
                // result.Token = jwt;
            }        
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
        }
        return result;

    }
    
}

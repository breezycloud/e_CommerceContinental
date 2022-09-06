using System.Net.Http.Json;
using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Services;

public class EmployeeService : IEmployeeService
{
    private readonly HttpClient _http;

    public EmployeeService(HttpClient http)
    {
        _http = http;
    }
    public async ValueTask<bool> AddEmployee(Employee employee)
    {
        using var response = await _http.PostAsJsonAsync("api/employee", employee);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async ValueTask<bool> DeleteEmployee(Guid id)
    {
        using var response = await _http.DeleteAsync($"api/employee/{id}");
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }
    public async ValueTask<bool> EditEmployee(Guid id, Employee employee)
    {
        using var response = await _http.PutAsJsonAsync($"api/employee/{id}", employee);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }
    public async ValueTask<Employee> GetEmployee(Guid id)
    {
        return await _http.GetFromJsonAsync<Employee>($"api/employee/{id}") ?? new Employee();
    }
    public async ValueTask<Employee[]> GetEmployees()
    {
        return await _http.GetFromJsonAsync<Employee[]>($"api/employee") ?? Array.Empty<Employee>();
    }
    public async ValueTask<Employee[]> GetEmployeeWithNoSalary()
    {
        return await _http.GetFromJsonAsync<Employee[]>($"api/employee/withnosalary") ?? Array.Empty<Employee>();
    }    
    public async ValueTask<Employee[]> GetEmployeesExcludingMaster()
    {
        return await _http.GetFromJsonAsync<Employee[]>($"api/employee/excludingmaster") ?? Array.Empty<Employee>();
    }
    public async ValueTask<int> GetTailorCurrentTask(Guid id)
    {
        return await _http.GetFromJsonAsync<int>($"api/employee/currenttasks/{id}");
    }
    public async ValueTask<Employee[]> GetTailors()
    {
        return await _http.GetFromJsonAsync<Employee[]>("api/employee/tailors") ?? Array.Empty<Employee>();
    }
    public async ValueTask<bool> CheckUsernameExist(string username)
    {
        return await _http.GetFromJsonAsync<bool>($"api/employee/UsernameExist/{username}");
    }
    public async ValueTask<bool> CreateEmployeeAccount(EmployeeAccount account)
    {
        using var request = await _http.PostAsJsonAsync("api/employee/registeraccount", account);
        request.EnsureSuccessStatusCode();
        return request.IsSuccessStatusCode;
    }
}
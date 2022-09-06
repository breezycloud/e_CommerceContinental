using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using e_CommerceContinental.Shared.Models;
using e_CommerceContinental.Client.Data;
using Blazored.LocalStorage;
using Microsoft.EntityFrameworkCore;

namespace e_CommerceContinental.Client.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DbContextProvider _context;
        private ILocalStorageService _localStorage;
        private HttpClient _httpClient;        
        public CustomerService(HttpClient httpClient, DbContextProvider context, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _context = context;
            _localStorage = localStorage;
        }

        public async Task<Customer> ExistCustomer(string model)
        {
            Customer customer = new();
            if (await _localStorage.GetItemAsync<bool>("mode"))                
            {
                customer = await _httpClient.GetFromJsonAsync<Customer>($"api/customers/existcustomer/{model}") ?? new Customer();
            }            
            else
            {
                await using var db  = await  _context.GetPreparedDbContextAsync();
                //customer = db.Customers.Where(x => x.PhoneNo == model).FirstOrDefault();                
            }
            return customer;
        }

        public async Task<bool> AddCustomer(Customer model)
        {            
            try
            {
                if (await _localStorage.GetItemAsync<bool>("mode"))                
                {
                    using var response = await _httpClient.PostAsJsonAsync("api/customers", model);
                    response.EnsureSuccessStatusCode();
                    return response.IsSuccessStatusCode;
                }    
                else
                {
                    await using var db  = await  _context.GetPreparedDbContextAsync();
                    // db.Customers.Add(model);
                    // var rowsAffected = await db.SaveChangesAsync();
                    // if (rowsAffected >= 1)
                    // {
                    //     var local = await _localStorage.GetItemAsync<int>("localChanges");
                    //     local++;
                    //     await _localStorage.SetItemAsync<int>("localChanges", local);
                    //     return true;
                    // }
                    // else
                    // {
                    //     return false;
                    // }
                    return false;
                }            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteCustomer(Customer model)
        {
            try
            {
                if (await _localStorage.GetItemAsync<bool>("mode"))                
                {                    
                    using var request = await _httpClient.DeleteAsync($"api/customers/{model.CustomerId}");
                    request.EnsureSuccessStatusCode();
                    return request.IsSuccessStatusCode;                    
                }                
                else
                {
                    await using var db = await _context.GetPreparedDbContextAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<Customer> GetCustomer(Customer model)
        {
            Customer customer = new();
            try
            {
                if (await _localStorage.GetItemAsync<bool>("mode"))
                {
                    customer = await _httpClient.GetFromJsonAsync<Customer>($"api/customers/{model.CustomerId}") ?? new Customer();                    
                }
                else
                {
                    await using var db = await _context.GetPreparedDbContextAsync();
                    // customer = await db.Customers.AsSplitQuery()
                    //                              .Include(x => x.CustomerMeasurements)
                    //                              .ThenInclude(x => x.Parts)
                    //                              .ThenInclude(x => x!.Group)
                    //                              .Where(x => x.CustomerId == model.CustomerId)
                    //                              .FirstOrDefaultAsync();    
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);            
            }
            return customer;
        }

        public async Task<Customer> GetCustomerWithMeasurement(Customer model)
        {
            Customer customer = new();
            try
            {
                if (await _localStorage.GetItemAsync<bool>("mode"))
                {
                    customer = await _httpClient.GetFromJsonAsync<Customer>($"api/customers/{model.CustomerId}") ?? new Customer();
                }
                else
                {
                    await using var db = await _context.GetPreparedDbContextAsync();
                    // customer = await db.Customers.Include(x => x.CustomerMeasurements)
                    //                              .Where(x => x.CustomerId == model.CustomerId)
                    //                              .FirstOrDefaultAsync();                    
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);            
            }
            return customer;            
        }
    

        public async Task<Customer[]> GetCustomers()
        {
            Customer[] customers = Array.Empty<Customer>();
            try
            {
                if (await _localStorage.GetItemAsync<bool>("mode"))
                {
                    customers = await _httpClient.GetFromJsonAsync<Customer[]>("api/customers") ?? Array.Empty<Customer>();                    
                }                
                else
                {
                    // using var db = await _context.GetPreparedDbContextAsync();                                        
                    // customers = await db.Customers.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return customers;
        }

        public async Task<bool> UpdateCustomer(Customer model)
        {
            bool status = false;
            try
            {  
                if (await _localStorage.GetItemAsync<bool>("mode"))              
                {
                    using var responseMessage = await _httpClient.PutAsJsonAsync($"api/customers/{model.CustomerId}", model);
                    responseMessage.EnsureSuccessStatusCode();
                    status = responseMessage.IsSuccessStatusCode;                
                }
                else
                {
                    // await using var db = await _context.GetPreparedDbContextAsync();
                    // db.Customers.Update(model);
                    // var rowsAffected = await db.SaveChangesAsync();                    
                    // if (rowsAffected >= 1)
                    // {
                    //     var local = await _localStorage.GetItemAsync<int>("localChanges");
                    //     local++;
                    //     await _localStorage.SetItemAsync<int>("localChanges", local);
                    //     status =  true;
                    // }
                    // else
                    // {
                    //     return false;
                    // }
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return status;
        }

        public async ValueTask<decimal> GetOutStandingBalance(Guid id)
        {
            decimal balance= 0;
            try
            {
                if (await _localStorage.GetItemAsync<bool>("mode"))
                {
                    balance = await _httpClient.GetFromJsonAsync<decimal>($"api/customers/outstandingbalance/{id}");
                }
                else
                {
                    // await using var db = await _context.GetPreparedDbContextAsync();
                    // balance = await db.Orders.Where(x => x.CustomerId == id).Select(x => x.Balance).SumAsync();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }            
            return balance;
        }

        public async Task<Customer[]> GetCustomersOnly()
        {
            Customer[] customers = Array.Empty<Customer>();
            try
            {
                if (await _localStorage.GetItemAsync<bool>("mode"))
                {
                    customers = await _httpClient.GetFromJsonAsync<Customer[]>("api/customers/only") ?? Array.Empty<Customer>();
                }
                else
                {
                    // await using var db = await _context.GetPreparedDbContextAsync();
                    // customers = await db.Customers.ToArrayAsync();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
            return customers;
        }        

        public async ValueTask<Customer> PhoneNumberExist(string phone)
        {
            Customer customer = new();
            try
            {
                if (await _localStorage.GetItemAsync<bool>("mode"))
                {
                    customer = await _httpClient.GetFromJsonAsync<Customer>($"api/customers/phone/{phone}") ?? new Customer();
                }
                else
                {
                    // await using var db = await _context.GetPreparedDbContextAsync();
                    // customer = await db.Customers.Where(x => x.PhoneNo == phone).FirstOrDefaultAsync();                    
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
            return customer;
        }

        public async Task<Dictionary<string, int>> GetTopServiceCustomer()
        {
            return await _httpClient.GetFromJsonAsync<Dictionary<string, int>>("api/customers/topservices") ?? new Dictionary<string, int>();
        }

        public async Task<Dictionary<string, int>> GetTopProductCustomer()
        {
            return await _httpClient.GetFromJsonAsync<Dictionary<string, int>>("api/customers/topproducts") ?? new Dictionary<string, int>();
        }
    }    
}
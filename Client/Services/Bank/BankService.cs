using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Services
{
    public class BankService : IBankService
    {
        private HttpClient _httpClient;

        public BankService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async ValueTask<bool> AddBank(Bank bank)
        {            
            using var request = await _httpClient.PostAsJsonAsync("api/banks", bank);            
            request.EnsureSuccessStatusCode();
            return request.IsSuccessStatusCode;
        }

        public async ValueTask<bool> DeleteBank(Bank bank)
        {
            using var request = await _httpClient.DeleteAsync($"api/banks/{bank.Id}");            
            request.EnsureSuccessStatusCode();
            return request.IsSuccessStatusCode;
        }

        public async ValueTask<Bank> GetBank(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Bank>($"api/banks/{id}") ?? new Bank();            
        }

        public async ValueTask<Bank[]> GetBanks()
        {
            return await _httpClient.GetFromJsonAsync<Bank[]>("api/banks") ?? Array.Empty<Bank>();            
        }

        public async ValueTask<bool> UpdateBank(Bank model)
        {            
            using var request = await _httpClient.PutAsJsonAsync($"api/banks/{model.Id}", model);            
            request.EnsureSuccessStatusCode();
            return request.IsSuccessStatusCode;
        }
    }
}

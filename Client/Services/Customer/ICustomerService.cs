using System.Collections.Generic;
using System.Threading.Tasks;
using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Services
{
    public interface ICustomerService
    {
        public Task<Customer[]> GetCustomers();
        public Task<Customer[]> GetCustomersOnly();
        public Task<Customer> GetCustomer(Customer model);
        public Task<Dictionary<string, int>> GetTopServiceCustomer();
        public Task<Dictionary<string, int>> GetTopProductCustomer();
        public ValueTask<decimal> GetOutStandingBalance(Guid id);
        public Task<Customer> GetCustomerWithMeasurement(Customer model);
        public Task<bool> AddCustomer(Customer model);
        public ValueTask<Customer> PhoneNumberExist(string phone);
        public Task<Customer> ExistCustomer(string model);
        public Task<bool> UpdateCustomer(Customer model);
        public Task<bool> DeleteCustomer(Customer model);        
    }   
}
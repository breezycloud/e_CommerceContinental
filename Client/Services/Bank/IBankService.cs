using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Services
{
    public interface IBankService
    {
        ValueTask<Bank[]> GetBanks();
        ValueTask<Bank> GetBank(Guid id);
        ValueTask<bool> AddBank(Bank bank);
        ValueTask<bool> UpdateBank(Bank bank);
        ValueTask<bool> DeleteBank(Bank bank);
    }
}

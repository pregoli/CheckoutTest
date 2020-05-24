using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.Domain.Entities;

namespace Checkout.Application.Common.Interfaces
{
    public interface ITransactionsService
    {
        Task<Transaction> AddAsync(Transaction transaction);
        Task<Transaction> GetAsync(Guid id);
        Task<List<Transaction>> GetTransactionsByMerchantIdAsync(Guid merchantId);
    }
}

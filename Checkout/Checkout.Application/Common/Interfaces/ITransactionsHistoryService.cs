using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.Domain.Entities;

namespace Checkout.Application.Common.Interfaces
{
    public interface ITransactionsHistoryService
    {
        Task<TransactionHistory> AddAsync(TransactionHistory transaction);
        Task<TransactionHistory> GetAsync(Guid id);
        Task<List<TransactionHistory>> GetTransactionsByMerchantIdAsync(Guid merchantId);
    }
}

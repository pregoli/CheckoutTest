using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.Application.Common.Dto;
using Checkout.Domain.Entities;

namespace Checkout.Application.Common.Interfaces
{
    public interface ITransactionsHistoryService
    {
        Task<TransactionItemDto> AddAsync(TransactionItemDto transactionItem);
        Task<TransactionItemDto> GetByTransactionIdAsync(Guid id);
        Task<List<TransactionItemDto>> GetByMerchantIdAsync(Guid id);
    }
}

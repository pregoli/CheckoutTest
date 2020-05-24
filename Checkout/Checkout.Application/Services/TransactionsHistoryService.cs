using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.Application.Common.Interfaces;
using Checkout.Domain.Entities;
using Checkout.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Application.Services
{
    public class TransactionsHistoryService : ITransactionsHistoryService
    {
        private readonly ITransactionsHistoryRepository _transactionsHistoryRepository;

        public TransactionsHistoryService(ITransactionsHistoryRepository transactionsHistoryRepository)
        {
            _transactionsHistoryRepository = transactionsHistoryRepository;
        }

        public async Task<TransactionHistory> AddAsync(TransactionHistory transaction)
        {
            return await _transactionsHistoryRepository.AddAsync(transaction);
        }

        public async Task<TransactionHistory> GetAsync(Guid id)
        {
            return await _transactionsHistoryRepository.GetAsync(id);
        }

        public async Task<List<TransactionHistory>> GetTransactionsByMerchantIdAsync(Guid merchantId)
        {
            return await _transactionsHistoryRepository.Query(x => x.MerchantId == merchantId).ToListAsync();
        }
    }
}

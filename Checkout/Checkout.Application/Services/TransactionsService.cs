using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.Application.Common.Interfaces;
using Checkout.Domain.Entities;
using Checkout.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Application.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsRepository _transactionsRepository;

        public TransactionsService(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            return await _transactionsRepository.AddAsync(transaction);
        }

        public async Task<Transaction> GetAsync(Guid id)
        {
            return await _transactionsRepository.GetAsync(id);
        }

        public async Task<List<Transaction>> GetTransactionsByMerchantIdAsync(Guid merchantId)
        {
            return await _transactionsRepository.Query(x => x.MerchantId == merchantId).ToListAsync();
        }
    }
}

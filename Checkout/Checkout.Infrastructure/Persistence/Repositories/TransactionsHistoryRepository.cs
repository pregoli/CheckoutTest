using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.Domain.Entities;
using Checkout.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Infrastructure.Persistence.Repositories
{
    public interface ITransactionsHistoryRepository
        : IRepository<TransactionHistory>
    {
        Task<List<TransactionHistory>> GetTransactionsByMerchantId(Guid merchantId);
    }

    public class TransactionsHistoryRepository : Repository<TransactionHistory>, ITransactionsHistoryRepository
    {
        public TransactionsHistoryRepository(CheckoutDbContext dbContext)
        : base(dbContext)
        {
        }

        public async Task<List<TransactionHistory>> GetTransactionsByMerchantId(Guid merchantId)
        {
            return await Query(x => x.MerchantId == merchantId).ToListAsync();
        }
    }
}

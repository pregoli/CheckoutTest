using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.Domain.Entities;
using Checkout.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Infrastructure.Persistence.Repositories
{
    public interface ITransactionsRepository : IRepository<Transaction>
    {
        Task<List<Transaction>> GetTransactionsByMerchantId(Guid merchantId);
    }

    public class TransactionsRepository : Repository<Transaction>, ITransactionsRepository
    {
        public TransactionsRepository(CheckoutDbContext dbContext)
        : base(dbContext)
        {
        }

        public async Task<List<Transaction>> GetTransactionsByMerchantId(Guid merchantId)
        {
            return await Query(x => x.MerchantId == merchantId).ToListAsync();
        }
    }
}

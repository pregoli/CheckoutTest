using System;
using System.Threading.Tasks;
using Checkout.Domain.Entities;
using Checkout.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Infrastructure.Persistence.Repositories
{
    public interface ITransactionsAuthRepository : IRepository<TransactionAuth>
    {
        Task<TransactionAuth> Get(decimal amount);
    }

    public class TransactionsAuthRepository : Repository<TransactionAuth>, ITransactionsAuthRepository
    {
        public TransactionsAuthRepository(CheckoutDbContext dbContext)
        : base(dbContext)
        {
        }

        public async Task<TransactionAuth> Get(decimal amount)
        {
            return await Query(x => amount.ToString().EndsWith(x.AmountEndWith)).FirstOrDefaultAsync();
        }
    }
}

using System.Threading.Tasks;
using Checkout.Domain.Entities;
using Checkout.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Infrastructure.Persistence.Repositories
{
    public interface IDevBankAuthRepository : IRepository<TransactionAuth>
    {
        Task<TransactionAuth> ValidateAsync(decimal amount);
    }

    public class DevBankAuthRepository : Repository<TransactionAuth>, IDevBankAuthRepository
    {
        public DevBankAuthRepository(CheckoutDbContext dbContext)
        : base(dbContext)
        {
        }

        public async Task<TransactionAuth> ValidateAsync(decimal amount)
        {
            return await Query(x => amount.ToString().EndsWith(x.AmountEndWith)).FirstOrDefaultAsync();
        }
    }
}

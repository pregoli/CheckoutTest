using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Checkout.Domain.Entities;

namespace Checkout.Infrastructure.Persistence.Repositories.Base
{
    public abstract class Repository<T> : IRepository<T>
        where T : class, IEntity
    {
        private readonly CheckoutDbContext context;
        public Repository(CheckoutDbContext context)
        {
            this.context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
            context.Set<T>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> where)
        {
            return context.Set<T>().Where(where).AsQueryable();
        }
    }
}

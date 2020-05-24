using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Checkout.Domain.Entities;

namespace Checkout.Infrastructure.Persistence.Repositories.Base
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T> GetAsync(Guid id);
        Task<T> AddAsync(T entity);
        IQueryable<T> Query(Expression<Func<T, bool>> where);
    }
}

using Checkout.Infrastructure.Persistence;
using Checkout.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.Infrastructure
{
    public static class Configuration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CheckoutDbContext>(opt =>
               opt.UseInMemoryDatabase("Checkout"));

            services.AddScoped<ITransactionsAuthRepository, TransactionsAuthRepository>();
            services.AddScoped<ITransactionsHistoryRepository, TransactionsHistoryRepository>();

            return services;
        }
    }
}

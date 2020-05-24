using Checkout.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Infrastructure.Persistence
{
    public class CheckoutDbContext : DbContext
    {
        public CheckoutDbContext(DbContextOptions<CheckoutDbContext> options)
            : base(options)
        {
        }
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Transaction>().ToTable("Transactions");

            //builder.Entity<CardDetails>()
            //.Property(cd => cd.CardDetailsID)
            //.ValueGeneratedOnAdd();

        }

    }
}

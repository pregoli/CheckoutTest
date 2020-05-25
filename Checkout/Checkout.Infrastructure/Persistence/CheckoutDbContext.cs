using System;
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
        public DbSet<TransactionHistory> Transactions { get; set; }
        public DbSet<TransactionAuth> TransactionResponseCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var merchant1 = "2769f5ea-1337-4f80-a1ba-35c35eded186";
            var merchant2 = "444998b1-10d1-498c-a1e1-8822cf08215b";
            var merchant3 = "a0a095c6-4056-41bb-aa4c-c1937252e964";
            var merchant4 = "f6a46846-5ca8-47ed-882b-f5d1a292c2ec";
            var merchant5 = "6c2b2591-dfa7-4981-9739-e6f39b486b59";
            
            var transactionId1 = "5ed79f45-e7fb-4944-aca1-46d0795512da";
            var transactionId2 = "b792055d-1172-4fa8-a659-e754a39f18dd";
            var transactionId3 = "33502eb0-5f48-4295-b428-e6117ef3ae86";
            var transactionId4 = "fa9056a3-4920-4f9c-ba3c-485d6d1f59f5";
            var transactionId5 = "ddc09ff7-6f82-4cd6-a46d-cbbb6ff087c8";

            builder.Entity<TransactionHistory>().HasData(new TransactionHistory (
                Guid.Parse(transactionId1), 
                Guid.Parse(merchant1),
                100,
                "Paolo Regoli",
                "1234567890123412",
                "10000",
                string.Empty));
            
            builder.Entity<TransactionHistory>().HasData(new TransactionHistory (
                Guid.Parse(transactionId2), 
                Guid.Parse(merchant2),
                100,
                "Paolo Kerry",
                "223456789012342",
                "10000",
                string.Empty));

            builder.Entity<TransactionHistory>().HasData(new TransactionHistory (
                Guid.Parse(transactionId3), 
                Guid.Parse(merchant3),
                100,
                "Paolo Murphy",
                "3234567890123412",
                "10000",
                string.Empty));

            builder.Entity<TransactionHistory>().HasData(new TransactionHistory (
                Guid.Parse(transactionId4), 
                Guid.Parse(merchant4),
                100,
                "Paolo Johnson",
                "4234567890123412",
                "10000",
                string.Empty));

            builder.Entity<TransactionHistory>().HasData(new TransactionHistory (
                Guid.Parse(transactionId5), 
                Guid.Parse(merchant5),
                100,
                "Paolo Elliot",
                "5234567890123412",
                "10000",
                string.Empty));

            builder.Entity<TransactionAuth>().HasData(
                new TransactionAuth(Guid.NewGuid(),"05", "20005", "Declined - Do not honour"),
                new TransactionAuth(Guid.NewGuid(),"12", "20012", "Invalid transaction"),
                new TransactionAuth(Guid.NewGuid(),"14", "20014", "Invalid card number"),
                new TransactionAuth(Guid.NewGuid(),"51", "20051", "Insufficient funds"),
                new TransactionAuth(Guid.NewGuid(),"54", "20087", "Bad track data"),
                new TransactionAuth(Guid.NewGuid(),"62", "20062", "Restricted card"),
                new TransactionAuth(Guid.NewGuid(),"63", "20063", "Security violation"),
                new TransactionAuth(Guid.NewGuid(),"9998", "20068", "Response received too late / timeout"),
                new TransactionAuth(Guid.NewGuid(),"150", "20150", "Card not 3D Secure enabled"),
                new TransactionAuth(Guid.NewGuid(),"6900", "20150", "Unable to specify if card is 3D Secure enabled"),
                new TransactionAuth(Guid.NewGuid(),"5000", "20153", "3D Secure system malfunction"),
                new TransactionAuth(Guid.NewGuid(),"5029", "20153", "3D Secure system malfunction"),
                new TransactionAuth(Guid.NewGuid(),"6510", "20154", "3D Secure Authentication Required"),
                new TransactionAuth(Guid.NewGuid(),"6520", "20154", "3D Secure Authentication Required"),
                new TransactionAuth(Guid.NewGuid(),"6530", "20154", "3D Secure Authentication Required"),
                new TransactionAuth(Guid.NewGuid(),"6540", "20154", "3D Secure Authentication Required"),
                new TransactionAuth(Guid.NewGuid(),"33", "30033", "Expired card - Pick up"),
                new TransactionAuth(Guid.NewGuid(),"4001", "40101", "Payment blocked due to risk"),
                new TransactionAuth(Guid.NewGuid(),"4008", "40108", "Gateway Reject – Post code failed"),
                new TransactionAuth(Guid.NewGuid(),"2011", "200R1", "Issuer initiated a stop payment (revocation order) for this authorization"),
                new TransactionAuth(Guid.NewGuid(),"2013", "200R3", "Issuer initiated a stop payment (revocation order) for all payments"));
        }
    }
}

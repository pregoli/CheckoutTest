using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkout.Domain.Entities
{
    public class Transaction : IEntity
    {
        public Transaction(
            Guid merchantId,
            decimal amount)
        {
            MerchantId = merchantId;
            Amount = amount;
            WhenCreated = DateTime.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public decimal Amount { get; set; }
        public DateTime WhenCreated { get; private set; }
    }
}

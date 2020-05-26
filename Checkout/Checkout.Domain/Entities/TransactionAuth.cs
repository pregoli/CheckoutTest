using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.Domain.Entities
{
    public class TransactionAuth : IEntity
    {
        public TransactionAuth(
            Guid id,
            string amountEndWith,
            string transactionCode,
            string description)
        {
            Id = id;
            AmountEndWith = amountEndWith;
            TransactionCode = transactionCode;
            Description = description;
        }

        [Key]
        public Guid Id { get; set; }
        public string AmountEndWith { get; set; }
        public string TransactionCode { get; set; }
        public string Description { get; set; }
    }
}

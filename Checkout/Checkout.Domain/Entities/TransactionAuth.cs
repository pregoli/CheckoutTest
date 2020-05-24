using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Checkout.Domain.Entities
{
    public class TransactionAuth : IEntity
    {
        public TransactionAuth(
            Guid transactionId,
            string amountEndWith,
            string responseCode,
            string description)
        {
            TransactionId = transactionId;
            AmountEndWith = amountEndWith;
            ResponseCode = responseCode;
            Description = description;
        }

        [Key]
        public Guid TransactionId { get; set; }
        public string AmountEndWith { get; set; }
        public string ResponseCode { get; set; }
        public string Description { get; set; }
    }
}

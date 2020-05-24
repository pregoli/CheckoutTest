using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.Domain.Entities
{
    public class TransactionHistory : IEntity
    {
        public TransactionHistory()
        {
        }

        public TransactionHistory(
            Guid transactionId,
            Guid merchantId,
            decimal amount,
            string cardHolderName,
            string cardNumber,
            string statusCode,
            string description,
            bool successful)
        {
            TransactionId = transactionId;
            MerchantId = merchantId;
            Amount = amount;
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            StatusCode = statusCode;
            Description = description;
            Successful = successful;
            Timestamp = DateTime.Now;
        }

        [Key]
        public Guid TransactionId { get; set; }
        public Guid MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public bool Successful { get; set; }
        public DateTime Timestamp { get; private set; }
    }
}

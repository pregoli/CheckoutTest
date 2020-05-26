using System;

namespace Checkout.Application.Common.ViewModels
{
    public class TransactionResponseVm
    {
        public TransactionResponseVm(
            Guid transactionId,
            Guid merchantId,
            string cardHolderName,
            string cardNumber,
            decimal amount,
            string statusCode,
            string description,
            DateTime timestamp)
        {
            TransactionId = transactionId;
            MerchantId = merchantId;
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            Amount = amount;
            StatusCode = statusCode;
            Description = description;
            Timestamp = timestamp;
            Currency = "GBP";
        }

        public Guid TransactionId { get; set; }
        public Guid MerchantId { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string Currency { get; private set; }
        public bool Successful => string.IsNullOrEmpty(Description);
    }
}

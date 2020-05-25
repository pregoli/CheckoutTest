using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Application.Common.Dto
{
    public class TransactionItemDto
    {
        public TransactionItemDto()
        {
        }

        public TransactionItemDto(
            Guid transactionId,
            Guid merchantId,
            decimal amount,
            string cardHolderName,
            string cardNumber,
            string statusCode,
            string description)
        {
            TransactionId = transactionId;
            MerchantId = merchantId;
            Amount = amount;
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            StatusCode = statusCode;
            Description = description;
            Timestamp = DateTime.Now;
        }

        public Guid TransactionId { get; set; }
        public Guid MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; private set; }
    }
}

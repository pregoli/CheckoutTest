using System;

namespace Checkout.Application.Common.Dto
{
    public class TransactionItemDto
    {
        public TransactionItemDto()
        {
        }

        public TransactionItemDto(
            Guid id,
            Guid merchantId,
            decimal amount,
            string cardHolderName,
            string cardNumber,
            string statusCode,
            string description)
        {
            Id = id;
            MerchantId = merchantId;
            Amount = amount;
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            StatusCode = statusCode;
            Description = description;
            Timestamp = DateTime.Now;
        }

        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public string Currency => "GBP";
        public decimal Amount { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; private set; }
    }
}

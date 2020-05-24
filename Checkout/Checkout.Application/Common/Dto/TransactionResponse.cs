using System;

namespace Checkout.Application.Common.Dto
{
    public class TransactionResponse
    {
        public Guid TransactionId { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string StatusCode { get; set; }
        public bool Successful { get; set; }
        public string Description { get; set; }
    }
}

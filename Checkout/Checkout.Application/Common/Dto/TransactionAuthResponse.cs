using System;

namespace Checkout.Application.Common.Dto
{
    public class TransactionAuthResponse
    {
        public TransactionAuthResponse(
            Guid transactionId,
            bool successful,
            string code,
            string description)
        {
            TransactionId = transactionId;
            Successful = successful;
            Code = code;
            Description = description;
        }

        public Guid TransactionId { get; set; }
        public bool Successful { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}

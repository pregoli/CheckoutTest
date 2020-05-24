using System;

namespace Checkout.Application.Common.Dto
{
    public class TransactionExecutionResponse
    {
        public TransactionExecutionResponse(
            Guid transactionId,
            string statusCode,
            string description,
            bool successful)
        {
            TransactionId = transactionId;
            StatusCode = statusCode;
            Description = description;
            Successful = successful;
        }

        public Guid TransactionId { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public bool Successful { get; set; }
    }
}

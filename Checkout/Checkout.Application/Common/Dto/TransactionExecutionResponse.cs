using System;

namespace Checkout.Application.Common.Dto
{
    public class TransactionExecutionResponse
    {
        public TransactionExecutionResponse(
            Guid transactionId,
            bool successfull)
        {
            TransactionId = transactionId;
            Successfull = successfull;
        }

        public Guid TransactionId { get; set; }
        public bool Successfull { get; set; }
    }
}

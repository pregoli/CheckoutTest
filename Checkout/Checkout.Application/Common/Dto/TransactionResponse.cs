using System;

namespace Checkout.Application.Common.Dto
{
    public class TransactionResponse
    {
        public Guid TransactionId { get; set; }
        public bool Successfull { get; set; }
    }
}

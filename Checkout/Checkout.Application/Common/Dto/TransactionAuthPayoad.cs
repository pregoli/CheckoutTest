using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Application.Common.Dto
{
    public class TransactionAuthPayoad
    {
        public TransactionAuthPayoad(
            CardDetails cardDetails,
            decimal amount)
        {
            CardDetails = cardDetails;
            Amount = amount;
        }

        public CardDetails CardDetails { get; set; }
        public decimal Amount { get; set; }
    }
}

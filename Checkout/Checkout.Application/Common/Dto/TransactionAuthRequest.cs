namespace Checkout.Application.Common.Dto
{
    public class TransactionAuthRequest
    {
        public TransactionAuthRequest(
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

namespace Checkout.Application.Common.Dto
{
    public class TransactionAuthorizationRequest
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string Cvv { get; set; }
        public decimal Amount { get; set; }
    }
}

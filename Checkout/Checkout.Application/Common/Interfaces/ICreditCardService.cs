namespace Checkout.Application.Common.Interfaces
{
    public interface ICreditCardService
    {
        bool IsValid(string creditCardNumber);
    }
}

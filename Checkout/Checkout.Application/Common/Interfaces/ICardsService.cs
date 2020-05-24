using Checkout.Application.Common.Dto;

namespace Checkout.Application.Common.Interfaces
{
    public interface ICardsService
    {
        bool Validate(CardDetails CardDetails);
        string Encrypt(string cardNumber);
        string Decrypt(string cardNumber);
    }
}

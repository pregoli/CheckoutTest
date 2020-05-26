using System.Threading.Tasks;
using Checkout.Application.Common.Dto;

namespace Checkout.Application.Common.Interfaces
{
    public interface IBankAuthProvider
    {
        Task<TransactionAuthResponse> VerifyAsync(TransactionAuthRequest payload);
    }
}

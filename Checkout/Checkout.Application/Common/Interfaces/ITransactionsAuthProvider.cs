using System.Threading.Tasks;
using Checkout.Application.Common.Dto;

namespace Checkout.Application.Common.Interfaces
{
    public interface ITransactionsAuthProvider
    {
        Task<TransactionAuthResponse> Authorize(TransactionAuthPayoad payload);
    }
}

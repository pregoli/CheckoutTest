using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.Application.Common.Dto;

namespace Checkout.Application.Common.Interfaces
{
    public interface ITransactionsAuthProvider
    {
        Task<TransactionAuthResponse> ProcessAsync(TransactionAuthPayoad payload);
    }
}

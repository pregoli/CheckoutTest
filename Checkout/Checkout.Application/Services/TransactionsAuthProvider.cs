using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Checkout.Infrastructure.Persistence.Repositories;
using System.Net;

namespace Checkout.Application.Services
{
    public class TransactionsAuthProvider : ITransactionsAuthProvider
    {
        private readonly ITransactionsAuthRepository _transactionsAuthRepository;
        private readonly ILogger<TransactionsAuthProvider> _logger;

        public TransactionsAuthProvider(
            ITransactionsAuthRepository transactionsAuthRepository,
            ILogger<TransactionsAuthProvider> logger)
        {
            _transactionsAuthRepository = transactionsAuthRepository;
            _logger = logger;
        }

        public async Task<TransactionAuthResponse> ProcessAsync(TransactionAuthPayoad payload)
        {
            if(payload.Amount <= 0)
                return new TransactionAuthResponse(
                    Guid.NewGuid(),
                    false,
                    HttpStatusCode.NotAcceptable.ToString(),
                    "Amount not accepted"
                );

            var transactionAuth = await _transactionsAuthRepository.Validate(payload.Amount);
            return transactionAuth == null ? 
                new TransactionAuthResponse(
                    Guid.NewGuid(),
                    true,
                    "10000",
                    "Successful"
                ) : 
                new TransactionAuthResponse(
                    Guid.NewGuid(),
                    false,
                    transactionAuth.ResponseCode,
                    transactionAuth.Description
                );
        }
    }
}

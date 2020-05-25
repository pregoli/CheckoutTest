using System;
using System.Threading.Tasks;
using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
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

        public async Task<TransactionAuthResponse> VerifyAsync(TransactionAuthPayload payload)
        {
            if(payload.Amount <= 0)
                return new TransactionAuthResponse(
                    Guid.NewGuid(),
                    false,
                    HttpStatusCode.NotAcceptable.ToString(),
                    "Amount not accepted"
                );

            try
            {
                var transactionAuth = await _transactionsAuthRepository.ValidateAsync(payload.Amount);
                return transactionAuth == null ? 
                    new TransactionAuthResponse(
                        Guid.NewGuid(),
                        true,
                        "10000",
                        "Successful"
                    ) : 
                    new TransactionAuthResponse(
                        transactionAuth.TransactionId,
                        false,
                        transactionAuth.ResponseCode,
                        transactionAuth.Description
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Unhandled Exception for Command {Name} {@Command}", 
                    nameof(TransactionsAuthProvider), 
                    payload);
            }

            return new TransactionAuthResponse(
                        Guid.NewGuid(),
                        true,
                        HttpStatusCode.ServiceUnavailable.ToString(),
                        "The verification could not be performed"
                    );
        }
    }
}

using System;
using System.Threading.Tasks;
using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Checkout.Infrastructure.Persistence.Repositories;
using System.Net;

namespace Checkout.Application.Services
{
    public class DevBankAuthProvider : IBankAuthProvider
    {
        private readonly IDevBankAuthRepository _bankAuthProvider;
        private readonly ILogger<DevBankAuthProvider> _logger;

        public DevBankAuthProvider(
            IDevBankAuthRepository bankAuthProvider,
            ILogger<DevBankAuthProvider> logger)
        {
            _bankAuthProvider = bankAuthProvider;
            _logger = logger;
        }

        public async Task<TransactionAuthResponse> VerifyAsync(TransactionAuthRequest payload)
        {
            var transactionId = Guid.NewGuid();
            if(payload.Amount <= 0)
                return new TransactionAuthResponse(
                    transactionId: Guid.NewGuid(),
                    verified: false,
                    code: HttpStatusCode.NotAcceptable.ToString(),
                    description: "Amount not accepted"
                );

            try
            {
                var transactionAuth = await _bankAuthProvider.ValidateAsync(payload.Amount);
                return transactionAuth == null ? 
                    new TransactionAuthResponse (
                        transactionId: transactionId,
                        verified: true,
                        code: "10000",
                        description: "Successful"
                    ) : 
                    new TransactionAuthResponse (
                        transactionId: transactionId,
                        verified: false,
                        code: transactionAuth.TransactionCode,
                        description: transactionAuth.Description
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Unhandled Exception for Command {Name} {@Command}", 
                    nameof(DevBankAuthProvider), 
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

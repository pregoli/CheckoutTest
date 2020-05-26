using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Interfaces;
using Checkout.Application.Events.Transactions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.Application.Commands.Transactions
{
    public class ExecutePayment : IRequest<Guid>
    {
        public Guid MerchantId { get; set; }
        public CardDetails CardDetails { get; set; }
        public decimal Amount { get; set; }
    }

    public class ExecutePaymentHandler : IRequestHandler<ExecutePayment, Guid>
    {
        private readonly ICardsService _cardsService;
        private readonly IBankAuthProvider _bankAuthProvider;
        private readonly IMediator _mediator;
        private readonly ILogger<ExecutePaymentHandler> _logger;

        public ExecutePaymentHandler(
            ICardsService cardsService,
            IBankAuthProvider bankAuthProvider, 
            IMediator mediator,
            ILogger<ExecutePaymentHandler> logger)
        {
            _cardsService = cardsService;
            _bankAuthProvider = bankAuthProvider;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Guid> Handle(ExecutePayment command, CancellationToken cancellationToken)
        {
            var transactionId = Guid.NewGuid();

            try
            {
                if (!_cardsService.Validate(command.CardDetails))
                {
                    var statusCode = HttpStatusCode.NotAcceptable.ToString();
                    var description = "Invalid card";

                    await _mediator.Publish(new PaymentExecuted(
                        transactionId: transactionId,
                        merchantId: command.MerchantId,
                        amount: command.Amount,
                        cardHolderName: command.CardDetails.CardHolderName,
                        encrypetdCardNumber: _cardsService.Encrypt(command.CardDetails?.CardNumber),
                        statusCode: statusCode,
                        description: description,
                        successful: false
                    ));

                    return transactionId;
                }
                
                var transactionAuthResponse = await _bankAuthProvider.VerifyAsync(new TransactionAuthRequest(
                        cardDetails: command.CardDetails, 
                        amount: command.Amount));

                transactionId = transactionAuthResponse.TransactionId;
                _mediator.Publish(new PaymentExecuted(
                    transactionId: transactionAuthResponse.TransactionId,
                    merchantId: command.MerchantId,
                    amount: command.Amount,
                    cardHolderName: command.CardDetails.CardHolderName,
                    encrypetdCardNumber: _cardsService.Encrypt(command.CardDetails.CardNumber),
                    statusCode: transactionAuthResponse.Code,
                    description: transactionAuthResponse.Description,
                    successful: transactionAuthResponse.Verified
                    ));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Unhandled Exception for Command {Name} {@Command}", 
                    nameof(ExecutePayment), 
                    command);

                var statusCode = HttpStatusCode.ServiceUnavailable.ToString();
                var description = "Something went wrong. Please try again later.";
                await _mediator.Publish(new PaymentExecuted(
                        transactionId: transactionId,
                        merchantId: command.MerchantId,
                        amount: command.Amount,
                        cardHolderName: command.CardDetails.CardHolderName,
                        encrypetdCardNumber: _cardsService.Encrypt(command.CardDetails?.CardNumber),
                        statusCode: statusCode,
                        description: description,
                        successful: false
                    ));
            }

            return transactionId;
        }
    }
}

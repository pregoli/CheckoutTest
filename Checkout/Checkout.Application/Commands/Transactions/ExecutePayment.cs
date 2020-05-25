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
    public class ExecutePayment : IRequest<PaymentExecutionResponse>
    {
        public Guid MerchantId { get; set; }
        public CardDetails CardDetails { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }

    public class ExecutePaymentHandler : IRequestHandler<ExecutePayment, PaymentExecutionResponse>
    {
        private readonly ICardsService _cardsService;
        private readonly ITransactionsAuthProvider _transactionsAuthProvider;
        private readonly IMediator _mediator;
        private readonly ILogger<ExecutePaymentHandler> _logger;

        public ExecutePaymentHandler(
            ICardsService cardsService,
            ITransactionsAuthProvider transactionsAuthProvider, 
            IMediator mediator,
            ILogger<ExecutePaymentHandler> logger)
        {
            _cardsService = cardsService;
            _transactionsAuthProvider = transactionsAuthProvider;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<PaymentExecutionResponse> Handle(ExecutePayment command, CancellationToken cancellationToken)
        {
            try
            {
                if (!_cardsService.Validate(command.CardDetails))
                {
                    var transactionId = Guid.NewGuid();
                    var code = HttpStatusCode.NotAcceptable.ToString();
                    var description = "Invalid card";

                    await _mediator.Publish(new PaymentExecuted(
                        transactionId: transactionId,
                        merchantId: command.MerchantId,
                        amount: command.Amount,
                        cardHolderName: command.CardDetails.CardHolderName,
                        encrypetdCardNumber: _cardsService.Encrypt(command.CardDetails?.CardNumber),
                        statusCode: code,
                        description: description,
                        successful: false
                    ));

                    return new PaymentExecutionResponse(transactionId, code, description, false);
                }
                
                var transactionAuthResponse = await _transactionsAuthProvider.VerifyAsync(new TransactionAuthPayload(
                        cardDetails: command.CardDetails, 
                        amount: command.Amount));

                await _mediator.Publish(new PaymentExecuted(
                    transactionId: transactionAuthResponse.TransactionId,
                    merchantId: command.MerchantId,
                    amount: command.Amount,
                    cardHolderName: command.CardDetails.CardHolderName,
                    encrypetdCardNumber: _cardsService.Encrypt(command.CardDetails.CardNumber),
                    statusCode: transactionAuthResponse.Code,
                    description: transactionAuthResponse.Description,
                    successful: transactionAuthResponse.Verified
                    ));

                return new PaymentExecutionResponse(
                        transactionId: transactionAuthResponse.TransactionId,
                        statusCode: transactionAuthResponse.Code,
                        description: transactionAuthResponse.Description,
                        successful: transactionAuthResponse.Verified);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Unhandled Exception for Command {Name} {@Command}", 
                    nameof(ExecutePayment), 
                    command);

                return new PaymentExecutionResponse(
                        transactionId: Guid.Empty,
                        statusCode: HttpStatusCode.ServiceUnavailable.ToString(),
                        description: "Something went wrong",
                        successful: false);
            }
        }
    }
}

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Checkout.Application.Common.Interfaces;
using MediatR;
using Checkout.Application.Common.Extensions;
using Microsoft.Extensions.Logging;
using Checkout.Application.Common.ViewModels;

namespace Checkout.Application.Queries.Transactions
{
    public class GetTransactionById : IRequest<TransactionResponseVm>
    {
        public Guid Id { get; set; }
    }

    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionById, TransactionResponseVm>
    {
        private readonly ICardsService _cardsService;
        private readonly ITransactionsHistoryService _transactionsHistoryService;
        private readonly ILogger<GetTransactionByIdQueryHandler> _logger;

        public GetTransactionByIdQueryHandler(
            ICardsService cardsService,
            ITransactionsHistoryService transactionsHistoryService,
            ILogger<GetTransactionByIdQueryHandler> logger)
        {
            _cardsService = cardsService;
            _transactionsHistoryService = transactionsHistoryService;
            _logger = logger;
        }

        public async Task<TransactionResponseVm> Handle(GetTransactionById request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _transactionsHistoryService.GetByTransactionIdAsync(request.Id);
                return result != null ? 
                    new TransactionResponseVm (
                        transactionId: result.Id,
                        merchantId: result.MerchantId,
                        cardHolderName: result.CardHolderName,
                        cardNumber: _cardsService.Decrypt(result.CardNumber).Mask('X'),
                        amount: result.Amount,
                        statusCode: result.StatusCode,
                        description: result.Description,
                        timestamp: result.Timestamp ) : 
                    new TransactionResponseVm (
                        transactionId: request.Id,
                        merchantId: Guid.Empty,
                        cardHolderName: string.Empty,
                        cardNumber: string.Empty,
                        amount: 0,
                        statusCode: HttpStatusCode.NotFound.ToString(),
                        description: "The requested transaction could not be found",
                        timestamp: DateTime.MinValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Checkout Request: Unhandled Exception for Request {Name} {@Request}",
                    nameof(GetTransactionById), 
                    request);
            }

            return new TransactionResponseVm (
                        transactionId: request.Id,
                        merchantId: Guid.Empty,
                        cardHolderName: string.Empty,
                        cardNumber: string.Empty,
                        amount: 0,
                        statusCode: HttpStatusCode.ServiceUnavailable.ToString(),
                        description: "Unfortunately It was not possible to process your request",
                        timestamp: DateTime.MinValue);
        }
    }
}

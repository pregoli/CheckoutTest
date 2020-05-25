using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Interfaces;
using MediatR;
using Checkout.Application.Common.Extensions;
using Microsoft.Extensions.Logging;

namespace Checkout.Application.Queries.Transactions
{
    public class GetTransactionById : IRequest<TransactionResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionById, TransactionResponse>
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

        public async Task<TransactionResponse> Handle(GetTransactionById request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _transactionsHistoryService.GetByTransactionIdAsync(request.Id);
                return result != null ? 
                    new TransactionResponse
                    { 
                        TransactionId = result.TransactionId,
                        Amount = result.Amount,
                        CardHolderName = result.CardHolderName,
                        CardNumber = _cardsService.Decrypt(result.CardNumber).Mask('X'),
                        Description = result.Description,
                        StatusCode = result.StatusCode
                    } : 
                    new TransactionResponse
                    { 
                        TransactionId = request.Id,
                        Amount = 0,
                        CardHolderName = string.Empty,
                        CardNumber = string.Empty,
                        Description = "The requested transaction could not be found",
                        StatusCode = HttpStatusCode.NotFound.ToString()
                    };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Checkout Request: Unhandled Exception for Request {Name} {@Request}",
                    nameof(GetTransactionById), 
                    request);
            }

            return new TransactionResponse
                { 
                    TransactionId = request.Id,
                    Amount = 0,
                    CardHolderName = string.Empty,
                    CardNumber = string.Empty,
                    Description = "Unfortunately It was not possible to process your request",
                    StatusCode = HttpStatusCode.ServiceUnavailable.ToString()
                };
        }
    }
}

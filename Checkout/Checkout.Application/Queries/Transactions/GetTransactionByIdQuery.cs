using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.Application.Queries.Transactions
{
    public class GetTransactionByIdQuery : IRequest<TransactionResponse>
    {
        public Guid TransactionId { get; set; }
    }

    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionResponse>
    {
        private readonly ITransactionsHistoryService _transactionsService;
        private readonly ILogger<GetTransactionByIdQueryHandler> _logger;

        public GetTransactionByIdQueryHandler(
            ITransactionsHistoryService transactionsService,
            ILogger<GetTransactionByIdQueryHandler> logger)
        {
            _transactionsService = transactionsService;
            _logger = logger;
        }

        public async Task<TransactionResponse> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _transactionsService.GetAsync(request.TransactionId);
                return new TransactionResponse
                    { 
                        TransactionId = result.TransactionId,
                        Amount = result.Amount,
                        CardHolderName = result.CardHolderName,
                        CardNumber = result.CardNumber,
                        Description = result.Description,
                        StatusCode = result.StatusCode,
                        Successful = result.Successful
                    };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Checkout Request: Unhandled Exception for Request {Name} {@Request}", 
                    nameof(GetTransactionByIdQuery), 
                    request);
            }

            return new TransactionResponse
                { 
                    TransactionId = request.TransactionId,
                    Amount = 0,
                    CardHolderName = string.Empty,
                    CardNumber = string.Empty,
                    Description = "Unfortunately It was not possible to process your request",
                    StatusCode = HttpStatusCode.ServiceUnavailable.ToString(),
                    Successful = false
                };
        }
    }
}

using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Extensions;
using Checkout.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Application.Queries.Transactions
{
    public class GetTransactionByMerchantId : IRequest<List<TransactionResponse>>
    {
        public Guid MerchantId { get; set; }
    }

    public class GetTransactionByMerchantIdQueryHandler : IRequestHandler<GetTransactionByMerchantId, List<TransactionResponse>>
    {
        private readonly ICardsService _cardsService;
        private readonly ITransactionsHistoryService _transactionsService;
        private readonly ILogger<GetTransactionByMerchantIdQueryHandler> _logger;

        public GetTransactionByMerchantIdQueryHandler(
            ICardsService cardsService,
            ITransactionsHistoryService transactionsService,
            ILogger<GetTransactionByMerchantIdQueryHandler> logger)
        {
            _cardsService = cardsService;
            _transactionsService = transactionsService;
            _logger = logger;
        }

        public async Task<List<TransactionResponse>> Handle(GetTransactionByMerchantId request, CancellationToken cancellationToken)
        {
            var response = new List<TransactionResponse>();
            
            try
            {
                var transactionItems = await _transactionsService.GetByMerchantIdAsync(request.MerchantId);
                transactionItems?.ForEach(transactionItem => response.Add(
                    new TransactionResponse
                    { 
                        TransactionId = transactionItem.TransactionId,
                        Amount = transactionItem.Amount,
                        CardHolderName = transactionItem.CardHolderName,
                        CardNumber = _cardsService.Decrypt(transactionItem.CardNumber).Mask('X'),
                        Description = transactionItem.Description,
                        StatusCode = transactionItem.StatusCode
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Checkout Request: Unhandled Exception for Request {Name} {@Request}", 
                    nameof(GetTransactionById), 
                    request);
            }

            return response;
        }
    }
}

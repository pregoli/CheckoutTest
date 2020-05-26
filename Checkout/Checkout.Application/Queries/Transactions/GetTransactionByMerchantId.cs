using Checkout.Application.Common.Extensions;
using Checkout.Application.Common.Interfaces;
using Checkout.Application.Common.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Application.Queries.Transactions
{
    public class GetTransactionByMerchantId : IRequest<List<TransactionResponseVm>>
    {
        public Guid MerchantId { get; set; }
    }

    public class GetTransactionByMerchantIdQueryHandler : IRequestHandler<GetTransactionByMerchantId, List<TransactionResponseVm>>
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

        public async Task<List<TransactionResponseVm>> Handle(GetTransactionByMerchantId request, CancellationToken cancellationToken)
        {
            var response = new List<TransactionResponseVm>();
            try
            {
                var transactionItems = await _transactionsService.GetByMerchantIdAsync(request.MerchantId);
                return transactionItems?.Select(transactionItem =>
                    new TransactionResponseVm (
                        transactionId: transactionItem.Id,
                        merchantId: transactionItem.MerchantId,
                        cardHolderName: transactionItem.CardHolderName,
                        cardNumber: _cardsService.Decrypt(transactionItem.CardNumber).Mask('X'),
                        amount: transactionItem.Amount,
                        statusCode: transactionItem.StatusCode,
                        description: transactionItem.Description,
                        timestamp: transactionItem.Timestamp)).ToList() ?? response;
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

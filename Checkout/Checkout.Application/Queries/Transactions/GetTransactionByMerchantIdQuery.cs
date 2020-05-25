using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Extensions;
using Checkout.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Application.Queries.Transactions
{
    public class GetTransactionByMerchantIdQuery : IRequest<List<TransactionResponse>>
    {
        public Guid MerchantId { get; set; }
    }

    public class GetTransactionByMerchantIdQueryHandler : IRequestHandler<GetTransactionByMerchantIdQuery, List<TransactionResponse>>
    {
        private readonly ICardsService _cardsService;
        private readonly ITransactionsHistoryService _transactionsService;
        private readonly ILogger<GetTransactionByMerchantIdQuery> _logger;

        public GetTransactionByMerchantIdQueryHandler(
            ICardsService cardsService,
            ITransactionsHistoryService transactionsService,
            ILogger<GetTransactionByMerchantIdQuery> logger)
        {
            _cardsService = cardsService;
            _transactionsService = transactionsService;
            _logger = logger;
        }

        public async Task<List<TransactionResponse>> Handle(GetTransactionByMerchantIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var transactionItems = await _transactionsService.GetByTransactionIdAsync(request.MerchantId);
                //return transactionItems != null ? 
                //    new TransactionResponse
                //    { 
                //        TransactionId = transactionItems.TransactionId,
                //        Amount = transactionItems.Amount,
                //        CardHolderName = transactionItems.CardHolderName,
                //        CardNumber = _cardsService.Decrypt(transactionItems.CardNumber).Mask('X'),
                //        Description = transactionItems.Description,
                //        StatusCode = transactionItems.StatusCode
                //    } : 
                //    new TransactionResponse
                //    { 
                //        TransactionId = request.MerchantId,
                //        Amount = 0,
                //        CardHolderName = string.Empty,
                //        CardNumber = string.Empty,
                //        Description = "We could not find any transaction for the given merchant",
                //        StatusCode = HttpStatusCode.NotFound.ToString()
                //    };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Checkout Request: Unhandled Exception for Request {Name} {@Request}", 
                    nameof(GetTransactionByIdQuery), 
                    request);
            }

            return new List<TransactionResponse>();
            //return new TransactionResponse
            //    { 
            //        TransactionId = request.MerchantId,
            //        Amount = 0,
            //        CardHolderName = string.Empty,
            //        CardNumber = string.Empty,
            //        Description = "Unfortunately It was not possible to process your request",
            //        StatusCode = HttpStatusCode.ServiceUnavailable.ToString()
            //    };
        }
    }
}

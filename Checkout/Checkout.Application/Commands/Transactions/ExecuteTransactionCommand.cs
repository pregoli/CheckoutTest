using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Interfaces;
using Checkout.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.Application.Commands.Transactions
{
    public class ExecuteTransactionCommand : IRequest<TransactionExecutionResponse>
    {
        public Guid MerchantId { get; set; }
        public CardDetails CardDetails { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }

    public class ExecutePaymentCommandHandler : IRequestHandler<ExecuteTransactionCommand, TransactionExecutionResponse>
    {
        private readonly ITransactionsHistoryService _transactionsHistoryService;
        private readonly ICardsService _cardsService;
        private readonly ITransactionsAuthProvider _transactionsAuthProvider;
        private readonly ILogger<ExecutePaymentCommandHandler> _logger;

        public ExecutePaymentCommandHandler(
            ITransactionsHistoryService transactionsHistoryService,
            ICardsService cardsService,
            ITransactionsAuthProvider transactionsAuthProvider,
            ILogger<ExecutePaymentCommandHandler> logger)
        {
            _transactionsHistoryService = transactionsHistoryService;
            _cardsService = cardsService;
            _transactionsAuthProvider = transactionsAuthProvider;
            _logger = logger;
        }

        public async Task<TransactionExecutionResponse> Handle(ExecuteTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            { 
                TransactionHistory transactionHistoryItem;
                if(!_cardsService.Validate(request.CardDetails))
                {
                    transactionHistoryItem = await _transactionsHistoryService.AddAsync(new TransactionHistory(
                        Guid.NewGuid(),
                        request.MerchantId,
                        request.Amount,
                        request.CardDetails.CardHolderName,
                        request.CardDetails.CardNumber,
                        HttpStatusCode.NotAcceptable.ToString(),
                        "Invalid card",
                        false
                        ));

                    return new TransactionExecutionResponse(
                        transactionHistoryItem.TransactionId,
                        transactionHistoryItem.StatusCode,
                        transactionHistoryItem.Description,
                        transactionHistoryItem.Successful);
                }
                
                var transactionAuthResponse = await _transactionsAuthProvider.ProcessAsync(new TransactionAuthPayoad(
                    request.CardDetails,
                    request.Amount));

                transactionHistoryItem = await _transactionsHistoryService.AddAsync(new TransactionHistory(
                    transactionAuthResponse.TransactionId,
                    request.MerchantId,
                    request.Amount,
                    request.CardDetails.CardHolderName,
                    request.CardDetails.CardNumber,
                    transactionAuthResponse.Code,
                    transactionAuthResponse.Description,
                    transactionAuthResponse.Successful));

                return new TransactionExecutionResponse(
                        transactionHistoryItem.TransactionId,
                        transactionHistoryItem.StatusCode,
                        transactionHistoryItem.Description,
                        transactionHistoryItem.Successful);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Checkout Request: Unhandled Exception for Request {Name} {@Request}", 
                    nameof(ExecuteTransactionCommand), 
                    request);
            }

            return null;
        }
    }
}

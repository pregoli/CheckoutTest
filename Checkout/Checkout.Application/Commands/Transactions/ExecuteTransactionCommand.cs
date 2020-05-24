using System;
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
        public string CardNumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Cvv { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }

    public class ExecutePaymentCommandHandler : IRequestHandler<ExecuteTransactionCommand, TransactionExecutionResponse>
    {
        private readonly ITransactionsService _transactionsService;
        private readonly ILogger<ExecutePaymentCommandHandler> _logger;

        public ExecutePaymentCommandHandler(
            ITransactionsService transactionsService,
            ILogger<ExecutePaymentCommandHandler> logger)
        {
            _transactionsService = transactionsService;
            _logger = logger;
        }

        public async Task<TransactionExecutionResponse> Handle(ExecuteTransactionCommand request, CancellationToken cancellationToken)
        {
            var entity = new Transaction(request.MerchantId, request.Amount);

            var transaction = await _transactionsService.AddAsync(entity);

            var test = await _transactionsService.GetAsync(transaction.Id);
            var test2 = await _transactionsService.GetTransactionsByMerchantIdAsync(transaction.MerchantId);

            return new TransactionExecutionResponse(transaction.Id, true);
        }
    }
}

using System;
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
        public Guid Id { get; set; }
    }

    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionResponse>
    {
        private readonly ITransactionsService _transactionsService;
        private readonly ILogger<GetTransactionByIdQueryHandler> _logger;

        public GetTransactionByIdQueryHandler(
            ITransactionsService transactionsService,
            ILogger<GetTransactionByIdQueryHandler> logger)
        {
            _transactionsService = transactionsService;
            _logger = logger;
        }

        public async Task<TransactionResponse> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _transactionsService.GetAsync(request.Id);
            return new TransactionResponse{ TransactionId = result.Id };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.Application.Commands.Transactions;
using Checkout.Application.Common.ViewModels;
using Checkout.Application.Queries.Transactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/beta/Transactions
        [HttpPost("beta/[controller]")]
        public async Task<ActionResult> ExecutePayment(ExecutePayment command)
        {
            var transactionId = await _mediator.Send(command);
            return RedirectToRoute(nameof(GetTransactionById), new { id = transactionId });
        }

        // Get: api/beta/Transactions/{id}
        [HttpGet("beta/[controller]/{id}", Name = nameof(GetTransactionById))]
        public async Task<ActionResult<TransactionResponseVm>> GetTransactionById(Guid id)
        {
            return await _mediator.Send(new GetTransactionById { Id = id });
        }
        
        // Get: api/beta/Merchants/{id}/Transactions
        [HttpGet("beta/Merchants/{id}/[controller]", Name = nameof(GetTransactionByMerchantId))]
        public async Task<ActionResult<List<TransactionResponseVm>>> GetTransactionByMerchantId(Guid id)
        {
            return await _mediator.Send(new GetTransactionByMerchantId { MerchantId = id });
        }
    }
}

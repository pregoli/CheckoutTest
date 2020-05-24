using System;
using System.Threading.Tasks;
using Checkout.Application.Commands.Transactions;
using Checkout.Application.Common.Dto;
using Checkout.Application.Queries.Transactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult<PaymentExecutionResponse>> ExecutePayment(ExecutePayment command)
        {
            return CreatedAtAction(nameof(ExecutePayment), await _mediator.Send(command));
        }

        // POST: api/Transactions/{id}
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<TransactionResponse>> GetTransactionById(Guid id)
        {
            return await _mediator.Send(new GetTransactionByIdQuery { TransactionId = id });
        }
    }
}

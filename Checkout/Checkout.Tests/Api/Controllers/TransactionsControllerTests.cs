using Checkout.Api.Controllers;
using Checkout.Application.Commands.Transactions;
using Checkout.Application.Common.Dto;
using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Tests.Api.Controllers
{
    [TestFixture]
    public class TransactionsControllerTests
    {
        ExecutePayment _command;
        private TransactionsController _controller;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();

            _command = new ExecutePayment
            {
                Amount = 100,
                CardDetails = new CardDetails(),
                Currency = "EUR",
                MerchantId = Guid.NewGuid()
            };

            _controller = new TransactionsController(_mediator.Object);
        }

        [Test]
        public async Task XXX()
        {
            //Arange
            PaymentExecutionResponse response = new PaymentExecutionResponse(
                        transactionId: Guid.Empty,
                        statusCode: HttpStatusCode.ServiceUnavailable.ToString(),
                        description: "Something went wrong",
                        successful: false);

            _mediator.Setup(x => x.Send(It.IsAny<ExecutePayment>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));

            //Act
            var result = await _controller.ExecutePayment(_command);

            //Assert
            Assert.NotNull(result);
        }
    }
}

using Checkout.Application.Commands.Transactions;
using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Interfaces;
using Checkout.Tests.Mock;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Tests.Application.Commands.Transactions
{
    [TestFixture]
    public class ExecutePaymentTests
    {
        private ExecutePayment _command;
        private ExecutePaymentHandler _handler;

        private Mock<ICardsService> _cardsService;
        private Mock<ITransactionsAuthProvider> _transactionsAuthProvider;
        private Mock<IMediator> _mediator;
        private Mock<LoggerMock<ExecutePaymentHandler>> _logger;

        [SetUp]
        public void Setup()
        {
            _cardsService = new Mock<ICardsService>();
            _transactionsAuthProvider = new Mock<ITransactionsAuthProvider>();
            _mediator = new Mock<IMediator>();
            _logger = new Mock<LoggerMock<ExecutePaymentHandler>>();

            _command = new ExecutePayment
            {
                Amount = 100,
                CardDetails = new CardDetails(),
                Currency = "EUR",
                MerchantId = Guid.NewGuid()
            };

            _handler = new ExecutePaymentHandler(
                _cardsService.Object,
                _transactionsAuthProvider.Object,
                _mediator.Object,
                _logger.Object);
        }

        [Test]
        public async Task For_A_Given_Exception_Thrown_By_AuthProvider_A_ServiceUnavailable_StatusCode_Is_Expected_From_Response()
        {
            //Arange
            _cardsService.Setup(x => x.Validate(It.IsAny<CardDetails>())).Returns(true);
            _transactionsAuthProvider.Setup(x => x.VerifyAsync(It.IsAny<TransactionAuthPayload>()))
                .Throws(new Exception());

            //Act
            var result = await _handler.Handle(_command, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
            Assert.False(result.Successful);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.ServiceUnavailable.ToString());
        }

        [Test]
        public async Task For_An_Invalid_Card_Data_A_NotAcceptable_StatusCode_Is_Expected_From_Response()
        {
            //Arange
            _cardsService.Setup(x => x.Validate(It.IsAny<CardDetails>())).Returns(false);

            //Act
            var result = await _handler.Handle(_command, new CancellationToken());

            //Assert
            _transactionsAuthProvider.Verify(x => x.VerifyAsync(It.IsAny<TransactionAuthPayload>()), Times.Never);
            Assert.NotNull(result);
            Assert.False(result.Successful);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.NotAcceptable.ToString());
        }

        [TestCase("20150", "Card not 3D Secure enabled")]
        [TestCase("20153", "3D Secure system malfunction")]
        [TestCase("20154", "3D Secure Authentication Required")]
        public async Task For_A_Not_Verified_Transaction_From_The_Auth_Service_An_Unsuccessful_Response_Is_Expected(
            string statusCode, string description)
        {
            //Arange
            _cardsService.Setup(x => x.Validate(It.IsAny<CardDetails>())).Returns(true);

            var transactionId = Guid.NewGuid();
            var transactionAuthResponse = new TransactionAuthResponse(transactionId, false, statusCode, description);
            _transactionsAuthProvider.Setup(x => x.VerifyAsync(It.IsAny<TransactionAuthPayload>()))
                .Returns(Task.FromResult(transactionAuthResponse));
            //Act
            var result = await _handler.Handle(_command, new CancellationToken());

            //Assert
            _transactionsAuthProvider.Verify(x => x.VerifyAsync(It.IsAny<TransactionAuthPayload>()), Times.Exactly(1));
            Assert.NotNull(result);
            Assert.False(result.Successful);
            Assert.AreEqual(result.StatusCode, statusCode);
        }

        [Test]
        public async Task For_A_Verified_Transaction_From_The_Auth_Service_A_Successful_Response_Is_Expected()
        {
            //Arange
            _cardsService.Setup(x => x.Validate(It.IsAny<CardDetails>())).Returns(true);

            var transactionId = Guid.NewGuid();
            var transactionAuthResponse = new TransactionAuthResponse(transactionId, true, "Successful", string.Empty);
            _transactionsAuthProvider.Setup(x => x.VerifyAsync(It.IsAny<TransactionAuthPayload>()))
                .Returns(Task.FromResult(transactionAuthResponse));
            //Act
            var result = await _handler.Handle(_command, new CancellationToken());

            //Assert
            _transactionsAuthProvider.Verify(x => x.VerifyAsync(It.IsAny<TransactionAuthPayload>()), Times.Exactly(1));
            Assert.NotNull(result);
            Assert.True(result.Successful);
            Assert.AreEqual(result.StatusCode, "Successful");
        }
    }
}

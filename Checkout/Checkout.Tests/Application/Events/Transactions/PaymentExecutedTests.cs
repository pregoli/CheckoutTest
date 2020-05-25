using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Interfaces;
using Checkout.Application.Events.Transactions;
using Checkout.Tests.Mock;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Tests.Application.Events.Transactions
{
    [TestFixture]
    public class PaymentExecutedTests
    {
        private PaymentExecuted _event;
        private PaymentExecutedHandler _handler;

        private Mock<ICardsService> _cardsService;
        private Mock<ITransactionsHistoryService> _transactionsHistoryService;
        private Mock<LoggerMock<PaymentExecutedHandler>> _logger;

        [SetUp]
        public void Setup()
        {
            _cardsService = new Mock<ICardsService>();
            _transactionsHistoryService = new Mock<ITransactionsHistoryService>();
            _logger = new Mock<LoggerMock<PaymentExecutedHandler>>();

            _event = new PaymentExecuted
                (
                Guid.NewGuid(),
                Guid.NewGuid(),
                100,
                "Paolo Regoli",
                "2222333344445555",
                "Successful",
                string.Empty,
                true);

            _handler = new PaymentExecutedHandler(
                _transactionsHistoryService.Object,
                _cardsService.Object,
                _logger.Object);
        }

        [Test]
        public async Task For_A_Given_Exception_Thrown_By_Service_An_Error_Will_Be_Logged()
        {
            //Arange
            _transactionsHistoryService.Setup(x => x.AddAsync(It.IsAny<TransactionItemDto>()))
                .Throws(new Exception());

            //Act
            await _handler.Handle(_event, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task For_A_Given_Response_From_The_Service_An_Exception_Will_Be_Not_Thrown()
        {
            //Arange
            _transactionsHistoryService.Setup(x => x.AddAsync(It.IsAny<TransactionItemDto>()))
                .Returns(Task.FromResult(new TransactionItemDto()));

            //Act
            await _handler.Handle(_event, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
        }
    }
}

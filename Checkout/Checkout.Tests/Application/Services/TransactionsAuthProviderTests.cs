using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Interfaces;
using Checkout.Application.Services;
using Checkout.Domain.Entities;
using Checkout.Infrastructure.Persistence.Repositories;
using Checkout.Tests.Mock;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Tests.Application.Services
{
    [TestFixture]
    public class TransactionsAuthProviderTests
    {
        private TransactionAuthPayload _transactionAuthPayload;
        private TransactionsAuthProvider _transactionsAuthProvider;

        private Mock<ITransactionsAuthRepository> _transactionsAuthRepository;
        private Mock<LoggerMock<TransactionsAuthProvider>> _logger;

        [SetUp]
        public void Setup()
        {
            _transactionsAuthRepository = new Mock<ITransactionsAuthRepository>();
            _logger = new Mock<LoggerMock<TransactionsAuthProvider>>();

            _transactionAuthPayload = new TransactionAuthPayload(new CardDetails(), 100);
            _transactionsAuthProvider = new TransactionsAuthProvider(
                _transactionsAuthRepository.Object,
                _logger.Object);
        }

        [Test]
        public async Task For_A_Given_Zero_Amount_A_NotAcceptable_StatusCode_From_Response_Is_Expected()
        {
            //Arange
            _transactionAuthPayload.Amount = 0;

            //Act
            var result = await _transactionsAuthProvider.VerifyAsync(_transactionAuthPayload);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.Code, HttpStatusCode.NotAcceptable.ToString());
        }

        [Test]
        public async Task For_A_Given_Exception_Thrown_By_Repository_A_ServiceUnavailable_StatusCode_From_Response_Is_Expected()
        {
            //Arange
            _transactionAuthPayload.Amount = 100;

            _transactionsAuthRepository.Setup(x => x.ValidateAsync(It.IsAny<decimal>()))
                .Throws(new Exception());

            //Act
            var result = await _transactionsAuthProvider.VerifyAsync(_transactionAuthPayload);

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
            Assert.AreEqual(result.Code, HttpStatusCode.ServiceUnavailable.ToString());
        }

        [Test]
        public async Task For_A_Given_Null_Response_By_Repository_A_Successful_StatusCode_Is_Expected()
        {
            //Arange
            _transactionAuthPayload.Amount = 100;

            TransactionAuth response = null;
            _transactionsAuthRepository.Setup(x => x.ValidateAsync(It.IsAny<decimal>()))
                .Returns(Task.FromResult(response));

            //Act
            var result = await _transactionsAuthProvider.VerifyAsync(_transactionAuthPayload);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.Code, "10000");
        }
        
        [Test]
        public async Task For_A_Given_Not_Null_Response_By_Repository_A_Successful_StatusCode_Is_Expected()
        {
            //Arange
            _transactionAuthPayload.Amount = 105;

            var transactionId = Guid.NewGuid();
            var response = new TransactionAuth(transactionId,"05", "20005", "Declined - Do not honour");
            _transactionsAuthRepository.Setup(x => x.ValidateAsync(It.IsAny<decimal>()))
                .Returns(Task.FromResult(response));

            //Act
            var result = await _transactionsAuthProvider.VerifyAsync(_transactionAuthPayload);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.TransactionId, transactionId);
            Assert.AreEqual(result.Code, response.ResponseCode);
        }
    }
}

using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Interfaces;
using Checkout.Application.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Tests.Application.Services
{
    [TestFixture]
    public class CardsServiceTests
    {
        private ICardsService _cardService;
        [OneTimeSetUp]
        public void Setup()
        { 
            _cardService = new CardsService();
        }

        [TestCase(null)]
        [TestCase("")]
        public void For_A_Null_Or_Empty_Card_Number_The_Validation_Will_Fail(string cardNumber)
        { 
            //Arrange
            var cardDetails = new CardDetails
            { 
                CardNumber = cardNumber
            };

            //Act
            var result = _cardService.Validate(cardDetails);

            //Assert
            Assert.False(result);
        }
        
        [TestCase("hello")]
        [TestCase("123412341234")]
        public void For_An_Invalid_Card_Number_The_Validation_Will_Fail(string cardNumber)
        { 
            //Arrange
            var cardDetails = new CardDetails
            { 
                CardNumber = cardNumber
            };

            //Act
            var result = _cardService.Validate(cardDetails);

            //Assert
            Assert.False(result);
        }
    }
}

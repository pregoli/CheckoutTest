﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Interfaces;

namespace Checkout.Application.Services
{
    public class CardsService : ICardsService
    {
        public bool Validate(CardDetails CardDetails)
        {
            if(CardDetails == null) return false;

            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");
            var cvvCheck = new Regex(@"^\d{3}$");


            int sumOfDigits = CardDetails.CardNumber.Where((e) => e >= '0' && e <= '9')
                    .Reverse()
                    .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                    .Sum((e) => e / 10 + e % 10);
       
            var cardNumberisValid = sumOfDigits % 10 == 0;            
            if (!cardNumberisValid)
                return false;
            if (!cvvCheck.IsMatch(CardDetails.Cvv))
                return false;

            if (!monthCheck.IsMatch(CardDetails.ExpirationMonth) || !yearCheck.IsMatch(CardDetails.ExpirationYear)) // <3 - 6>
                return false; 

            var year = int.Parse(CardDetails.ExpirationYear);
            var month = int.Parse(CardDetails.ExpirationMonth);            
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get actual expiry date
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
        }
    }
}
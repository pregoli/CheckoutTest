﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.Domain.Entities
{
    public class TransactionHistory : IEntity
    {
        public TransactionHistory()
        {
        }

        public TransactionHistory(
            Guid id,
            Guid merchantId,
            decimal amount,
            string cardHolderName,
            string cardNumber,
            string statusCode,
            string description)
        {
            Id = id;
            MerchantId = merchantId;
            Amount = amount;
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            StatusCode = statusCode;
            Description = description;
            Timestamp = DateTime.Now;
        }

        [Key]
        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public string Currency => "GBP";
        public decimal Amount { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public bool Successful => string.IsNullOrEmpty(Description);
        public DateTime Timestamp { get; private set; }
    }
}

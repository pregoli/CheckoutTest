using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.Application.Common.Interfaces;
using Checkout.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.Application.Events.Transactions
{
    public class PaymentExecuted : INotification
    {
        public PaymentExecuted(
            Guid transactionId,
            Guid merchantId,
            decimal amount,
            string cardHolderName,
            string encrypetdCardNumber,
            string statusCode,
            string description,
            bool successful)
        {
            TransactionId = transactionId;
            MerchantId = merchantId;
            Amount = amount;
            CardHolderName = cardHolderName;
            EncrypetdCardNumber = encrypetdCardNumber;
            StatusCode = statusCode;
            Description = description;
            Successful = successful;
        }

        public Guid TransactionId { get; set; }
        public Guid MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string CardHolderName { get; set; }
        public string EncrypetdCardNumber { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public bool Successful { get; set; }
    }

    public class PaymentExecutedHandler : INotificationHandler<PaymentExecuted>
    {
        private readonly ITransactionsHistoryService _transactionsHistoryService;
        private readonly ICardsService _cardsService;
        private readonly ILogger<PaymentExecutedHandler> _logger;

        public PaymentExecutedHandler(
            ITransactionsHistoryService transactionsHistoryService, 
            ICardsService cardsService,
            ILogger<PaymentExecutedHandler> logger)
        {
            _transactionsHistoryService = transactionsHistoryService;
            _cardsService = cardsService;
            this._logger = logger;
        }

        public async Task Handle(PaymentExecuted @event, CancellationToken cancellationToken)
        {
            try
            {
                await _transactionsHistoryService.AddAsync(
                    new TransactionHistory(
                    @event.TransactionId,
                    @event.MerchantId,
                    @event.Amount,
                    @event.CardHolderName,
                    @event.EncrypetdCardNumber,
                    @event.StatusCode,
                    @event.Description,
                    @event.Successful));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Unhandled Exception for Event {Name} {@Event}", 
                    nameof(PaymentExecuted), 
                    @event);
            }
        }
    }
}

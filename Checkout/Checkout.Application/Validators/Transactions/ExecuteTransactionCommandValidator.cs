using Checkout.Application.Commands.Transactions;
using Checkout.Application.Common.Dto;
using FluentValidation;

namespace Checkout.Application.Validators.Transactions
{
    public class ExecuteTransactionCommandValidator : AbstractValidator<ExecuteTransactionCommand>
    {
        public ExecuteTransactionCommandValidator()
        {
            RuleFor(v => v.Amount > 0);
            RuleFor(v => v.MerchantId)
                .NotNull()
                .NotEmpty();
            RuleFor(v => v.CardDetails)
                .SetValidator(new CardDetailsValidator())
                .NotEmpty();
        }
    }

    public class CardDetailsValidator : AbstractValidator<CardDetails>
    {
        public CardDetailsValidator()
        {
            RuleFor(v => v.CardHolderName)
                .NotNull()
                .NotEmpty();
            RuleFor(v => v.Cvv)
                .NotNull()
                .NotEmpty()
                .Length(3);
            RuleFor(v => v.CardNumber)
                .NotNull()
                .NotEmpty()
                .Length(16);
            RuleFor(v => v.ExpirationMonth)
                .NotNull()
                .NotEmpty()
                .Length(2);
            RuleFor(v => v.ExpirationYear)
                .NotNull()
                .NotEmpty()
                .Length(4);
        }
    }
}

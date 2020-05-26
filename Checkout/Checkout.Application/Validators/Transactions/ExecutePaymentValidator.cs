using Checkout.Application.Commands.Transactions;
using Checkout.Application.Validators.Cards;
using FluentValidation;
using System;

namespace Checkout.Application.Validators.Transactions
{
    public class ExecutePaymentValidator : AbstractValidator<ExecutePayment>
    {
        public ExecutePaymentValidator()
        {
            RuleFor(v => v.Amount)
                .GreaterThan(0);
            RuleFor(v => v.MerchantId)
                .NotNull()
                .NotEmpty()
                .NotEqual(Guid.Empty);
            RuleFor(v => v.CardDetails)
                .NotNull()
                .SetValidator(new CardDetailsValidator())
                .NotEmpty();
        }
    }
}

using Checkout.Application.Commands.Transactions;
using FluentValidation;

namespace Checkout.Application.Validators.Transactions
{
    public class ExecuteTransactionCommandValidator : AbstractValidator<ExecuteTransactionCommand>
    {
        public ExecuteTransactionCommandValidator()
        {
            RuleFor(v => v.Amount > 0);
            RuleFor(v => v.Cvv)
                .Length(3)
                .NotEmpty();
            RuleFor(v => v.CardNumber)
                .Length(16)
                .NotEmpty();
            RuleFor(v => v.ExpirationMonth)
                .Length(2)
                .NotEmpty();
            RuleFor(v => v.ExpirationYear)
                .Length(4)
                .NotEmpty();
            RuleFor(v => v.MerchantId)
                .NotNull()
                .NotEmpty();
        }
    }
}

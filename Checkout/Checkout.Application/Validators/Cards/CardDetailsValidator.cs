using Checkout.Application.Common.Dto;
using FluentValidation;

namespace Checkout.Application.Validators.Cards
{
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
                .Must(x => int.TryParse(x, out _))
                .Length(3);
            RuleFor(v => v.CardNumber)
                .CreditCard()
                .NotNull()
                .NotEmpty()
                .Length(16);
            RuleFor(v => v.ExpirationMonth)
                .NotNull()
                .NotEmpty()
                .Must(x => int.TryParse(x, out _))
                .Length(2);
            RuleFor(v => v.ExpirationYear)
                .NotNull()
                .NotEmpty()
                .Must(x => int.TryParse(x, out _))
                .Length(4);
        }
    }
}

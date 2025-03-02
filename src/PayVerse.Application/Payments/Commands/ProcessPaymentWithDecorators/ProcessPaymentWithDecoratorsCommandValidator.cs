using FluentValidation;

namespace PayVerse.Application.Payments.Commands.ProcessPaymentWithDecorators;

internal sealed class ProcessPaymentWithDecoratorsCommandValidator : AbstractValidator<ProcessPaymentWithDecoratorsCommand>
{
    public ProcessPaymentWithDecoratorsCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithMessage("Payment ID is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage("Currency is required")
            .Length(3)
            .WithMessage("Currency code must be 3 characters long")
            .Matches("^[A-Z]{3}$")
            .WithMessage("Currency must be in ISO 4217 format (e.g., USD, EUR)");

        RuleFor(x => x.Provider)
            .NotEmpty()
            .WithMessage("Provider is required");

        RuleFor(x => x.PaymentDetails)
            .NotNull()
            .WithMessage("Payment details are required");

        When(x => x.PaymentDetails != null, () =>
        {
            RuleFor(x => x.PaymentDetails)
                .Must(details => details.ContainsKey("returnUrl"))
                .WithMessage("Return URL is required in payment details")
                .Must(details => details.ContainsKey("cancelUrl"))
                .WithMessage("Cancel URL is required in payment details");
        });
    }
}

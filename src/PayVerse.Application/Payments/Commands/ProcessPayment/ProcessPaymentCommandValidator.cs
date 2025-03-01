using FluentValidation;

namespace PayVerse.Application.Payments.Commands.ProcessPayment;

internal sealed class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
{
    public ProcessPaymentCommandValidator()
    {
        RuleFor(command => command.PaymentId)
            .NotEmpty().WithMessage("Payment ID cannot be empty.");

        RuleFor(command => command.ProviderName)
            .NotEmpty().WithMessage("Provider name cannot be empty.");

        RuleFor(command => command.PaymentDetails)
            .NotEmpty().WithMessage("Payment details cannot be empty.");
    }
}
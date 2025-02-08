using FluentValidation;

namespace PayVerse.Application.Payments.Commands.RetryFailedPayment;

internal class RetryFailedPaymentCommandValidator : AbstractValidator<RetryFailedPaymentCommand>
{
    public RetryFailedPaymentCommandValidator()
    {
        RuleFor(cmd => cmd.PaymentId).NotEmpty();
    }
}
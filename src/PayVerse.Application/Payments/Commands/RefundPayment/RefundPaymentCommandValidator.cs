using FluentValidation;

namespace PayVerse.Application.Payments.Commands.RefundPayment;

internal class RefundPaymentCommandValidator : AbstractValidator<RefundPaymentCommand>
{
    public RefundPaymentCommandValidator()
    {
        RuleFor(cmd => cmd.PaymentId).NotEmpty();
    }
}
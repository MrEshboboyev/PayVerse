using FluentValidation;
using PayVerse.Application.Payments.Commands.CreatePayment;

namespace PayVerse.Application.Payments.Commands.UpdatePaymentStatus;

internal class UpdatePaymentStatusCommandValidator : AbstractValidator<UpdatePaymentStatusCommand>
{
    public UpdatePaymentStatusCommandValidator()
    {
        RuleFor(cmd => cmd.PaymentId)
            .NotEmpty().WithMessage("Payment ID is required.");

        RuleFor(cmd => cmd.Status)
            .IsInEnum().WithMessage("Payment status must be a valid status.");
    }
}

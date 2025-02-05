using FluentValidation;

namespace PayVerse.Application.Payments.Commands.CreatePayment;

internal class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(cmd => cmd.Amount)
            .GreaterThan(0).WithMessage("Payment amount must be greater than zero.");

        RuleFor(cmd => cmd.Status)
            .IsInEnum().WithMessage("Payment status must be a valid status.");

        RuleFor(cmd => cmd.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}

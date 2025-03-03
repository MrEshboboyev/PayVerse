using FluentValidation;

namespace PayVerse.Application.Payments.Commands.InitiatePayment;

internal class InitiatePaymentCommandValidator : AbstractValidator<InitiatePaymentCommand>
{
    public InitiatePaymentCommandValidator()
    {
        RuleFor(cmd => cmd.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(cmd => cmd.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(cmd => cmd.ScheduledDate)
            .Must(date => !date.HasValue || date.Value > DateTime.UtcNow)
            .WithMessage("Scheduled date must be in the future.");
    }
}

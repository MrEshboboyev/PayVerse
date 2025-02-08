using FluentValidation;

namespace PayVerse.Application.Payments.Commands.SchedulePayment;

internal class SchedulePaymentCommandValidator : AbstractValidator<SchedulePaymentCommand>
{
    public SchedulePaymentCommandValidator()
    {
        RuleFor(cmd => cmd.Amount).GreaterThan(0);
        RuleFor(cmd => cmd.UserId).NotEmpty();
        RuleFor(cmd => cmd.ScheduledDate).GreaterThan(DateTime.UtcNow);
    }
}
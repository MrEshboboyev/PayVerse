using FluentValidation;

namespace PayVerse.Application.Notifications.Commands.MarkNotificationAsRead;

internal class MarkNotificationAsReadCommandValidator : AbstractValidator<MarkNotificationAsReadCommand>
{
    public MarkNotificationAsReadCommandValidator()
    {
        RuleFor(cmd => cmd.NotificationId)
            .NotEmpty().WithMessage("Notification ID is required.");
    }
}
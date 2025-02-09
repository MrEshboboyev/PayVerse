using FluentValidation;

namespace PayVerse.Application.Notifications.Commands.SendNotification;

internal class SendNotificationCommandValidator : AbstractValidator<SendNotificationCommand>
{
    public SendNotificationCommandValidator()
    {
        RuleFor(cmd => cmd.NotificationId)
            .NotEmpty().WithMessage("Notification ID is required.");
    }
}
using FluentValidation;

namespace PayVerse.Application.Notifications.Commands.UpdateNotificationMessage;

internal class UpdateNotificationMessageCommandValidator : AbstractValidator<UpdateNotificationMessageCommand>
{
    public UpdateNotificationMessageCommandValidator()
    {
        RuleFor(cmd => cmd.NotificationId)
            .NotEmpty().WithMessage("Notification ID is required.");

        RuleFor(cmd => cmd.NewMessage)
            .NotEmpty().WithMessage("New notification message is required.")
            .MaximumLength(255).WithMessage("New notification message cannot exceed 255 characters.");
    }
}
using FluentValidation;
using PayVerse.Domain.ValueObjects.Notifications;

namespace PayVerse.Application.Notifications.Commands.CreateNotification;

internal class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(cmd => cmd.NotificationMessage)
            .NotEmpty().WithMessage("Notification message is required.")
            .MaximumLength(NotificationMessage.MaxLength).WithMessage("Notification message cannot exceed 255 characters.");

        RuleFor(cmd => cmd.Type)
            .NotEmpty().WithMessage("Notification type is required.");

        RuleFor(cmd => cmd.Priority)
            .NotEmpty().WithMessage("Notification priority is required.");

        RuleFor(cmd => cmd.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(cmd => cmd.DeliveryMethod)
            .NotEmpty().WithMessage("Notification delivery method is required.");
    }
}
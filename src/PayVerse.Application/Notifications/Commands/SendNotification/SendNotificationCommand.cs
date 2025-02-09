using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Notifications.Commands.SendNotification;

public sealed record SendNotificationCommand(
    Guid NotificationId) : ICommand;
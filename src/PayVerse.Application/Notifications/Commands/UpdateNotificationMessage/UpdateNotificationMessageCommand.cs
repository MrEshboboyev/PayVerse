using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Notifications.Commands.UpdateNotificationMessage;

public sealed record UpdateNotificationMessageCommand(
    Guid NotificationId,
    string NewMessage) : ICommand;
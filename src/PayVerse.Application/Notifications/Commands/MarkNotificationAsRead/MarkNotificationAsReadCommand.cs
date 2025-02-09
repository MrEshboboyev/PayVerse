using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Notifications.Commands.MarkNotificationAsRead;

public sealed record MarkNotificationAsReadCommand(
    Guid NotificationId) : ICommand;
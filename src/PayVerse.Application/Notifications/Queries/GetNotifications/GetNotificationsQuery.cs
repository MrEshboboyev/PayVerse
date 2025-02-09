using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Notifications.Queries.Common.Responses;

namespace PayVerse.Application.Notifications.Queries.GetNotifications;

public sealed record GetNotificationsQuery(Guid UserId) : IQuery<NotificationListResponse>;
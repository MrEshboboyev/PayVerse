namespace PayVerse.Application.Notifications.Queries.Common.Responses;

public sealed record NotificationListResponse(
    IReadOnlyList<NotificationResponse> Notifications);
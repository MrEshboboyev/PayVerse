using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Notifications.Queries.Common.Factories;
using PayVerse.Application.Notifications.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Notifications;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Notifications.Queries.GetNotifications;

internal sealed class GetNotificationsQueryHandler(INotificationRepository notificationRepository)
    : IQueryHandler<GetNotificationsQuery, NotificationListResponse>
{
    public async Task<Result<NotificationListResponse>> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var notifications = await notificationRepository.GetByUserIdAsync(
            request.UserId,
            cancellationToken);
        
        var response = new NotificationListResponse(
            notifications.Select(NotificationResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        return Result.Success(response);
    }
}

using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Notifications;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Notifications.Commands.MarkNotificationAsRead;

internal sealed class MarkNotificationAsReadCommandHandler(
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<MarkNotificationAsReadCommand>
{
    public async Task<Result> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        var notificationId = request.NotificationId;
        
        #region Get this notification
        
        var notification = await notificationRepository.GetByIdAsync(
            notificationId,
            cancellationToken);
        if (notification is null)
        {
            return Result.Failure(
                DomainErrors.Notification.NotFound(notificationId));
        }
        
        #endregion
        
        #region Mark notification as read

        var result = notification.MarkAsRead();
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }
        
        #endregion

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
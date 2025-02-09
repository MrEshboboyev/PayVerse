using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Notifications;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Notifications.Commands.SendNotification;

internal sealed class SendNotificationCommandHandler(
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<SendNotificationCommand>
{
    public async Task<Result> Handle(
        SendNotificationCommand request,
        CancellationToken cancellationToken)
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
        
        #region Send notification

        var result = notification.Send();
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }
        
        #endregion

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
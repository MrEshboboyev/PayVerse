using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Notifications;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Notifications;

namespace PayVerse.Application.Notifications.Commands.UpdateNotificationMessage;

internal sealed class UpdateNotificationMessageCommandHandler(
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateNotificationMessageCommand>
{
    public async Task<Result> Handle(UpdateNotificationMessageCommand request, CancellationToken cancellationToken)
    {
        var (notificationId, newMessage) = request;
        
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
        
        #region Prepare value objects

        var newMessageResult = NotificationMessage.Create(newMessage);
        if (newMessageResult.IsFailure)
        {
            return Result.Failure(newMessageResult.Error);
        }
        
        #endregion
        
        #region Update message

        var result = notification.UpdateMessage(newMessageResult.Value);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }
        
        #endregion

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
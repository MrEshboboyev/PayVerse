using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Entities.Notifications;
using PayVerse.Domain.Enums.Notifications;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Notifications;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Notifications;

namespace PayVerse.Application.Notifications.Commands.CreateNotification;

internal sealed class CreateNotificationCommandHandler(
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateNotificationCommand>
{
    public async Task<Result> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var (notificationMessage, type, priority, userId, deliveryMethod) = request;
        
        #region Prepare value objects
        
        var messageResult = NotificationMessage.Create(notificationMessage);
        if (messageResult.IsFailure)
        {
            return Result.Failure(messageResult.Error);
        }   
        
        #endregion
        
        #region Create new notification

        var notification = Notification.Create(
            Guid.NewGuid(),
            messageResult.Value,
            request.Type,
            request.Priority,
            NotificationStatus.Pending,
            request.UserId,
            request.DeliveryMethod);
        
        #endregion

        await notificationRepository.AddAsync(notification, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
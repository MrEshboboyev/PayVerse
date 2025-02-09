using PayVerse.Domain.Errors;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.ValueObjects.Notifications;

public sealed class NotificationMessage : ValueObject
{
    #region Constants
    
    public const int MaxLength = 500;
    
    #endregion
    
    #region Properties
    
    public string Value { get; }
    
    #endregion

    #region Constructors
    
    private NotificationMessage(string value) => Value = value;
    
    #endregion
    
    #region Factory Methods

    public static Result<NotificationMessage> Create(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return Result.Failure<NotificationMessage>(
                DomainErrors.NotificationMessage.Empty(message));
        }

        if (message.Length > MaxLength)
        {
            return Result.Failure<NotificationMessage>(
                DomainErrors.NotificationMessage.TooLong(message, MaxLength));
        }

        return Result.Success(new NotificationMessage(message));
    }

    #endregion
    
    #region Overrides

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}
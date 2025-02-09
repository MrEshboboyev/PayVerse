using PayVerse.Domain.Primitives;

namespace PayVerse.Domain.Entities.Security;

public sealed class AuditLog : Entity
{
    #region Constructors

    internal AuditLog(Guid id,
        Guid userId,
        string action,
        string details,
        string ipAddress,
        string deviceInfo) 
        : base(id)
    {
        UserId = userId;
        Action = action;
        Details = details;
        IpAddress = ipAddress;
        DeviceInfo = deviceInfo;
        Timestamp = DateTime.UtcNow;
    }

    #endregion

    #region Properties

    public Guid UserId { get; private set; }
    public string Action { get; private set; } // e.g., "UserLoggedIn", "PaymentProcessed"
    public string Details { get; private set; }
    public string IpAddress { get; private set; }
    public string DeviceInfo { get; private set; }
    public DateTime Timestamp { get; private set; }

    #endregion

    #region Factory Methods

    public static AuditLog Record(
        Guid id,
        Guid userId,
        string action,
        string details,
        string ipAddress,
        string deviceInfo)
    {
        return new AuditLog(id, userId, action, details, ipAddress, deviceInfo);
    }

    #endregion
}

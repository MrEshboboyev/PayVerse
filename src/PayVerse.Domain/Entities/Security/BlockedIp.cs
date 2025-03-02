using PayVerse.Domain.Primitives;

namespace PayVerse.Domain.Entities.Security;

public sealed class BlockedIp : Entity
{
    #region Constructors

    internal BlockedIp(
        Guid id,
        string ipAddress,
        string reason,
        DateTime? expiryDate)
        : base(id)
    {
        IpAddress = ipAddress;
        Reason = reason;
        ExpiryDate = expiryDate;
    }

    #endregion

    #region Properties

    public Guid UserId { get; private set; }
    public string IpAddress { get; private set; }
    public string Reason { get; private set; }
    public DateTime? ExpiryDate { get; private set; }

    #endregion

    #region Factory Methods

    public static BlockedIp Create(
        Guid id,
        string ipAddress,
        string reason,
        DateTime? expiryDate)
    {
        return new BlockedIp(id, ipAddress, reason, expiryDate);
    }

    #endregion
}

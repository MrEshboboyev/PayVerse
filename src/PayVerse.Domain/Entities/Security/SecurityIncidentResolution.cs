using PayVerse.Domain.Primitives;

namespace PayVerse.Domain.Entities.Security;

public sealed class SecurityIncidentResolution : Entity
{
    #region Constructors

    internal SecurityIncidentResolution(
        Guid id,
        Guid securityIncidentId,
        string resolutionDetails,
        Guid resolvedBy)
        : base(id)
    {
        SecurityIncidentId = securityIncidentId;
        ResolutionDetails = resolutionDetails;
        ResolvedBy = resolvedBy;
        ResolvedAt = DateTime.UtcNow;
    }

    #endregion

    #region Properties

    public Guid SecurityIncidentId { get; private set; }
    public string ResolutionDetails { get; private set; }
    public Guid ResolvedBy { get; private set; }
    public DateTime ResolvedAt { get; private set; }

    #endregion
}

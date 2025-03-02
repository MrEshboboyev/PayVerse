using PayVerse.Domain.Enums.Security;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Events.Security;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.Entities.Security;

public sealed class SecurityIncident : AggregateRoot
{
    #region Constructors

    private SecurityIncident(
        Guid id,
        SecurityIncidentType type,
        string description,
        Guid? userId,
        string ipAddress)
        : base(id)
    {
        Type = type;
        Description = description;
        UserId = userId;
        IpAddress = ipAddress;
        Status = SecurityIncidentStatus.Pending;
        OccurredAt = DateTime.UtcNow;

        #region Domain Events

        RaiseDomainEvent(new SecurityIncidentLoggedDomainEvent(
            Guid.NewGuid(),
            Id));

        #endregion
    }

    #endregion

    #region Properties

    public SecurityIncidentType Type { get; private set; }
    public string Description { get; private set; }
    public Guid? UserId { get; private set; }
    public string IpAddress { get; private set; }
    public SecurityIncidentStatus Status { get; private set; }
    public DateTime OccurredAt { get; private set; }

    #endregion

    #region Factory Methods

    public static SecurityIncident Create(
        Guid id,
        SecurityIncidentType type,
        string description,
        Guid? userId,
        string ipAddress)
    {
        return new SecurityIncident(id, type, description, userId, ipAddress);
    }

    #endregion

    #region Methods

    public Result Resolve(
        string resolutionDetails,
        Guid resolvedBy)
    {
        if (Status is SecurityIncidentStatus.Resolved)
        {
            return Result.Failure(
                DomainErrors.SecurityIncident.IncidentAlreadyResolved(Id));
        }

        Status = SecurityIncidentStatus.Resolved;

        RaiseDomainEvent(new SecurityIncidentResolvedDomainEvent(
            Guid.NewGuid(),
            Id,
            resolutionDetails,
            resolvedBy));
        
        return Result.Success();
    }

    public Result Escalate()
    {
        if (Status is SecurityIncidentStatus.Escalated)
        {
            return Result.Failure(
                DomainErrors.SecurityIncident.IncidentAlreadyEscalated(Id));
        }

        Status = SecurityIncidentStatus.Escalated;
        
        return Result.Success();
    }

    #endregion

    #region Resolution related methods
    
    public Result<SecurityIncidentResolution> AddResolution(
        Guid id,
        string details,
        Guid resolvedBy)
    {
        if (Status is not SecurityIncidentStatus.Resolved)
        {
            return Result.Failure<SecurityIncidentResolution>(
                DomainErrors.SecurityIncident.IncidentNotResolved(Id));
        }

        var resolution = new SecurityIncidentResolution(
            id,
            Id,
            details,
            resolvedBy);

        // Add resolution logic here
        return Result.Success(resolution);
    }

    #endregion
}

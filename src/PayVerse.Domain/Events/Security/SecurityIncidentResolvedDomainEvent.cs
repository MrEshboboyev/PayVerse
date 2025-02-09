namespace PayVerse.Domain.Events.Security;

public sealed record SecurityIncidentResolvedDomainEvent(
    Guid Id,
    Guid SecurityIncidentId,
    string ResolutionDetails,
    Guid ResolvedBy) : DomainEvent(Id);
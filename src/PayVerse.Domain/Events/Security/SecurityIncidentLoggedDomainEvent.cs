namespace PayVerse.Domain.Events.Security;

public sealed record SecurityIncidentLoggedDomainEvent(
    Guid Id,
    Guid SecurityIncidentId) : DomainEvent(Id);
namespace PayVerse.Domain.Events.VirtualAccounts;

public sealed record VirtualAccountClosedDomainEvent(
    Guid Id, 
    Guid VirtualAccountId) : DomainEvent(Id);
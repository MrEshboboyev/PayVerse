namespace PayVerse.Domain.Events.VirtualAccounts;

public sealed record VirtualAccountCreatedDomainEvent(
    Guid Id, 
    Guid VirtualAccountId,
    string AccountNumber) : DomainEvent(Id);
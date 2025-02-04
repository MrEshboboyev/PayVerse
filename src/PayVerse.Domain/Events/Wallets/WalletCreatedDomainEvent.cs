namespace PayVerse.Domain.Events.Wallets;

public record WalletCreatedDomainEvent(
    Guid Id,
    Guid WalletId) : DomainEvent(Id);
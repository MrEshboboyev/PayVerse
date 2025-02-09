namespace PayVerse.Domain.Events.Wallets;

public sealed record LoyaltyPointsRedeemedDomainEvent(
    Guid Id,
    Guid WalletId,
    int PointsRedeemed) : DomainEvent(Id);
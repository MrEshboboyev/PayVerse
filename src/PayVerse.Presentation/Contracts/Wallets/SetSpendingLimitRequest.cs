namespace PayVerse.Presentation.Contracts.Wallets;

public sealed record SetSpendingLimitRequest(
    decimal SpendingLimit);
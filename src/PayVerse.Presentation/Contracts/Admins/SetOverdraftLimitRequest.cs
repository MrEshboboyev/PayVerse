namespace PayVerse.Presentation.Contracts.Admins;

public sealed record SetOverdraftLimitRequest(
    Guid AccountId,
    decimal OverdraftLimit);
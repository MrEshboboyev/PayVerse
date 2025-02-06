namespace PayVerse.Presentation.Contracts.VirtualAccounts;

public sealed record CreateVirtualAccountRequest(
    Guid UserId,
    string AccountNumber,
    string CurrencyCode,
    decimal Balance);
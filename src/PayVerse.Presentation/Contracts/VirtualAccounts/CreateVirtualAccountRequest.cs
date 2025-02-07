namespace PayVerse.Presentation.Contracts.VirtualAccounts;

public sealed record CreateVirtualAccountRequest(
    string AccountNumber,
    string CurrencyCode,
    decimal Balance);
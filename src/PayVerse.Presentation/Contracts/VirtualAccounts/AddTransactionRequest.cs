namespace PayVerse.Presentation.Contracts.VirtualAccounts;

public sealed record AddTransactionRequest(
    decimal Amount,
    DateTime Date,
    string Description);
namespace PayVerse.Presentation.Contracts.VirtualAccounts;

public sealed record TransferFundsRequest(
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount);
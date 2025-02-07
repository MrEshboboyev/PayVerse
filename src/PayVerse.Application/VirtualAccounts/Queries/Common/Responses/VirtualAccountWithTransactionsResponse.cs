namespace PayVerse.Application.VirtualAccounts.Queries.Common.Responses;

public sealed record VirtualAccountWithTransactionsResponse(
    VirtualAccountResponse VirtualAccount,
    IReadOnlyList<TransactionResponse> Transactions);
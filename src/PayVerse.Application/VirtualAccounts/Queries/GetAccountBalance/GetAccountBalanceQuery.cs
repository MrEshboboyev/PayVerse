using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.VirtualAccounts.Queries.GetAccountBalance;

public sealed record GetAccountBalanceQuery(
    Guid AccountId) : IQuery<decimal>;
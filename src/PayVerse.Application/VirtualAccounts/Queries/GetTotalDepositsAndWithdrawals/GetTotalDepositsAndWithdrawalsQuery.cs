using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;

namespace PayVerse.Application.VirtualAccounts.Queries.GetTotalDepositsAndWithdrawals;

public sealed record GetTotalDepositsAndWithdrawalsQuery(
    Guid AccountId) : IQuery<TotalDepositsAndWithdrawalsResponse>;
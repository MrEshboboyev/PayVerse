using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;

namespace PayVerse.Application.VirtualAccounts.Queries.GetTransactionHistoryByDate;

public sealed record GetTransactionHistoryByDateQuery(
    Guid AccountId,
    DateTime StartDate,
    DateTime EndDate) : IQuery<TransactionListResponse>;
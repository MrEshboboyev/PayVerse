using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;

namespace PayVerse.Application.VirtualAccounts.Queries.GetTransactions;

public sealed record GetTransactionsQuery(
    Guid VirtualAccountId) : IQuery<TransactionListResponse>;
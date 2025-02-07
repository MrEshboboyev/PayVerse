using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;

namespace PayVerse.Application.VirtualAccounts.Queries.GetVirtualAccountWithTransactionsById;

public sealed record GetVirtualAccountWithTransactionsByIdQuery(
    Guid VirtualAccountId) : IQuery<VirtualAccountWithTransactionsResponse>;

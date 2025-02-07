using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;

namespace PayVerse.Application.VirtualAccounts.Queries.GetTransactionById;

public sealed record GetTransactionByIdQuery(
    Guid VirtualAccountId,
    Guid TransactionId) : IQuery<TransactionResponse>;
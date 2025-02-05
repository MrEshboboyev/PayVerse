using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;

namespace PayVerse.Application.VirtualAccounts.Queries.GetVirtualAccountById;

public sealed record GetVirtualAccountByIdQuery(
    Guid VirtualAccountId) : IQuery<VirtualAccountResponse>;
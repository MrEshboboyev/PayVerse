using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;

namespace PayVerse.Application.VirtualAccounts.Queries.GetVirtualAccountsByUserId;

public sealed record GetVirtualAccountsByUserIdQuery(
    Guid UserId) : IQuery<VirtualAccountListResponse>;

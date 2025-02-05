using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;

namespace PayVerse.Application.VirtualAccounts.Queries.GetAllVirtualAccounts;

public sealed record GetAllVirtualAccountsQuery() : IQuery<VirtualAccountListResponse>;

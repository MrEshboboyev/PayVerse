using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Wallets.Queries.Common.Responses;

namespace PayVerse.Application.Wallets.Queries.GetWalletsByUserId;

public sealed record GetWalletsByUserIdQuery(
    Guid UserId) : IQuery<WalletListResponse>;

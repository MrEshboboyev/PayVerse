using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Wallets.Queries.Common.Responses;

namespace PayVerse.Application.Wallets.Queries.GetAllWallets;

public sealed record GetAllWalletsQuery() : IQuery<WalletListResponse>;
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Wallets.Queries.Common.Responses;

namespace PayVerse.Application.Wallets.Queries.GetWalletById;

public sealed record GetWalletByIdQuery(Guid WalletId) : IQuery<WalletResponse>;
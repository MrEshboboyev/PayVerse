using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Wallets.Queries.Common.Responses;

namespace PayVerse.Application.Wallets.Queries.GetWalletWithTransactionsById;

public sealed record GetWalletWithTransactionsByIdQuery(Guid WalletId) : IQuery<WalletWithTransactionsResponse>;
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Wallets.Queries.Common.Responses;

namespace PayVerse.Application.Wallets.Queries.GetWalletTransactions;

public sealed record GetWalletTransactionsQuery(
    Guid WalletId) : IQuery<WalletTransactionListResponse>;
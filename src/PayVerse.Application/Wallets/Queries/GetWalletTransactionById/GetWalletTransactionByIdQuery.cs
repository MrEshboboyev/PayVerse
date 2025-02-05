using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Wallets.Queries.Common.Responses;

namespace PayVerse.Application.Wallets.Queries.GetWalletTransactionById;

public sealed record GetWalletTransactionByIdQuery(
    Guid WalletId,
    Guid TransactionId) : IQuery<WalletTransactionResponse>;

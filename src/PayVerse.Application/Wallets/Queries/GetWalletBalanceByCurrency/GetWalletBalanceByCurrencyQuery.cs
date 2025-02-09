using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Wallets.Queries.GetWalletBalanceByCurrency;

public sealed record GetWalletBalanceByCurrencyQuery(
    Guid WalletId,
    string CurrencyCode) : IQuery<decimal>;
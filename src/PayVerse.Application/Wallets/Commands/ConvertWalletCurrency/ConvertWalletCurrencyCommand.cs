using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Wallets.Commands.ConvertWalletCurrency;

public sealed record ConvertWalletCurrencyCommand(
    Guid WalletId,
    string NewCurrencyCode) : ICommand;
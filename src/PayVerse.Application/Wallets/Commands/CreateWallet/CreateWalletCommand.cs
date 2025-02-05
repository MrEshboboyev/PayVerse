using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Wallets.Commands.CreateWallet;

public sealed record CreateWalletCommand(
    Guid UserId,
    decimal InitialBalance,
    string CurrencyCode) : ICommand;
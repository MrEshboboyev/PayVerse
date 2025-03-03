using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Wallets.Commands.AddFundsToWallet;

public sealed record AddFundsToWalletCommand(
    Guid WalletId,
    decimal Amount,
    string Description) : ICommand;

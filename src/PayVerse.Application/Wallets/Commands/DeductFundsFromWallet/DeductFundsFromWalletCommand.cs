using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Wallets.Commands.DeductFundsFromWallet;

public sealed record DeductFundsFromWalletCommand(
    Guid WalletId,
    decimal Amount,
    string Description) : ICommand;

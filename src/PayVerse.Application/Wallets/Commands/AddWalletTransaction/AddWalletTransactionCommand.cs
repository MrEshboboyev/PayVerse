using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Wallets.Commands.AddWalletTransaction;

public sealed record AddWalletTransactionCommand(
    Guid WalletId,
    decimal Amount,
    DateTime Date,
    string Description) : ICommand;
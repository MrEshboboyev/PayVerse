using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Wallets.Commands.RemoveWalletTransaction;

public sealed record RemoveWalletTransactionCommand(
    Guid WalletId,
    Guid TransactionId) : ICommand;
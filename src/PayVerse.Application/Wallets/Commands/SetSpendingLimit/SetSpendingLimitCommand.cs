using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Wallets.Commands.SetSpendingLimit;

public sealed record SetSpendingLimitCommand(
    Guid WalletId,
    decimal SpendingLimit) : ICommand;
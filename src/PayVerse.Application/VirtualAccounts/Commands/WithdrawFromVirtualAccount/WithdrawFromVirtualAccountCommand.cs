using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.VirtualAccounts.Commands.WithdrawFromVirtualAccount;

public sealed record WithdrawFromVirtualAccountCommand(
    Guid AccountId,
    decimal Amount,
    string Description) : ICommand;

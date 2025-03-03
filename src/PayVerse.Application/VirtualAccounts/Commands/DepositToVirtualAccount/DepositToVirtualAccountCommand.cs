using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.VirtualAccounts.Commands.DepositToVirtualAccount;

public sealed record DepositToVirtualAccountCommand(
    Guid AccountId,
    decimal Amount,
    string Description) : ICommand;

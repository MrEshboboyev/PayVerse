using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.VirtualAccounts.Commands.UnfreezeVirtualAccount;

public sealed record UnfreezeVirtualAccountCommand(
    Guid AccountId) : ICommand;

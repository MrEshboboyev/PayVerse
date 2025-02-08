using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.VirtualAccounts.Commands.FreezeVirtualAccount;

public sealed record FreezeVirtualAccountCommand(
    Guid AccountId) : ICommand;
    
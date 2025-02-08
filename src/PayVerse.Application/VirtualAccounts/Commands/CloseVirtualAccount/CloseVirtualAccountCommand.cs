using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.VirtualAccounts.Commands.CloseVirtualAccount;

public sealed record CloseVirtualAccountCommand(
    Guid AccountId) : ICommand;
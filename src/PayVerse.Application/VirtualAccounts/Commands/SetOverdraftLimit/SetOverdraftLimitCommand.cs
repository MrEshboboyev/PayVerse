using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.VirtualAccounts.Commands.SetOverdraftLimit;

public sealed record SetOverdraftLimitCommand(
    Guid AccountId,
    decimal OverdraftLimit) : ICommand;
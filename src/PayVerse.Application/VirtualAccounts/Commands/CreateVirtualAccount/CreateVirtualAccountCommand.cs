using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.VirtualAccounts.Commands.CreateVirtualAccount;

public sealed record CreateVirtualAccountCommand(
    string CurrencyCode,
    Guid UserId) : ICommand;
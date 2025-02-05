using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.VirtualAccounts.Commands.CreateVirtualAccount;

public sealed record CreateVirtualAccountCommand(
    string AccountNumber,
    string CurrencyCode,
    decimal Balance,
    Guid UserId) : ICommand;
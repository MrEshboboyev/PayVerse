using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.VirtualAccounts.Commands.TransferBetweenAccounts;

public sealed record TransferBetweenAccountsCommand(
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Description) : ICommand;

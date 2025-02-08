using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.VirtualAccounts.Commands.TransferFunds;

public sealed record TransferFundsCommand(
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount) : ICommand;
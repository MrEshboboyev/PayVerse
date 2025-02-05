using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.VirtualAccounts.Commands.AddTransaction;

public sealed record AddTransactionCommand(
    Guid VirtualAccountId,
    decimal Amount,
    DateTime Date,
    string Description) : ICommand;
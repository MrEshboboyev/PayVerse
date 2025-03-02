using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Security.Commands.TransferFundsWithDecorators;

public sealed record TransferFundsWithDecoratorsCommand(
    Guid SourceAccountId,
    Guid DestinationAccountId,
    decimal Amount,
    string Description) : ICommand<TransferFundsWithDecoratorsResult>;

namespace PayVerse.Application.Security.Commands.TransferFundsWithDecorators;

public sealed record TransferFundsWithDecoratorsResult(
    Guid TransactionId,
    bool Success,
    string Message);
namespace PayVerse.Domain.Decorators.Payments;

public sealed record PaymentResult(
    bool Success,
    string TransactionId,
    string Message);
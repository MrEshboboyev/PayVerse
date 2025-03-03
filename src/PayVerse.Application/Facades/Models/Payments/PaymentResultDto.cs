namespace PayVerse.Application.Facades.Models.Payments;

public sealed record PaymentResultDto(
    bool Success,
    Guid PaymentId,
    Guid InvoiceId,
    decimal Amount,
    DateTime TransactionDate,
    string Description,
    string ErrorMessage
);


namespace PayVerse.Application.Payments.Commands.ProcessPayment;

public sealed record PaymentDto(
    Guid PaymentId,
    string ProviderName,
    DateTime ProcessedDate,
    decimal Amount,
    bool IsSuccessful,
    string ErrorMessage
);
namespace PayVerse.Application.Facades.Models.Payments;

public sealed record InvoicePaymentResultDto(
    Guid InvoiceId,
    string InvoiceNumber,
    Guid PaymentId,
    DateTime ScheduledPaymentDate,
    string Status
);


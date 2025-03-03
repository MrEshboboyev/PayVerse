namespace PayVerse.Presentation.Contracts.Payments;

public sealed record RefundPaymentRequest(
    string RefundTransactionId,
    string Reason);

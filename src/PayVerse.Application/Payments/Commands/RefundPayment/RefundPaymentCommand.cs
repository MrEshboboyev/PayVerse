using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Payments.Commands.RefundPayment;

public sealed record RefundPaymentCommand(
    Guid PaymentId,
    string RefundTransactionId,
    string Reason) : ICommand;
using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Payments.Commands.RetryFailedPayment;

public sealed record RetryFailedPaymentCommand(
    Guid PaymentId) : ICommand;
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Decorators.Payments;

namespace PayVerse.Application.Payments.Commands.CustomizedProcessPaymentWithDecorators;

public sealed record CustomizedProcessPaymentCommand(
    Guid PaymentId,
    decimal Amount,
    bool EnableNotifications) : ICommand<PaymentResult>;
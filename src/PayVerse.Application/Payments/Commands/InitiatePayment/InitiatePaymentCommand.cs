using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Application.Payments.Commands.InitiatePayment;

public sealed record InitiatePaymentCommand(
    decimal Amount,
    Guid UserId,
    Guid? InvoiceId,
    PaymentMethod PaymentMethod,
    DateTime? ScheduledDate = null) : ICommand<Guid>;

using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Application.Payments.Commands.SchedulePayment;

public sealed record SchedulePaymentCommand(
    decimal Amount,
    PaymentStatus Status,
    Guid UserId,
    DateTime ScheduledDate) : ICommand;
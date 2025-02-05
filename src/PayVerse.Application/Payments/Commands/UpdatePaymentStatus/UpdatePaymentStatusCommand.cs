using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Application.Payments.Commands.UpdatePaymentStatus;

public sealed record UpdatePaymentStatusCommand(
    Guid PaymentId,
    PaymentStatus Status) : ICommand;
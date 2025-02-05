using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.ValueObjects.Payments;

namespace PayVerse.Application.Payments.Commands.CreatePayment;

public sealed record CreatePaymentCommand(
    decimal Amount,
    PaymentStatus Status,
    Guid UserId) : ICommand;
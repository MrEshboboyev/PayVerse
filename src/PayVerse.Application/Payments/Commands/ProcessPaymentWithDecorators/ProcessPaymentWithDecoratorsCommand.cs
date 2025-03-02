using MediatR;
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Decorators.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Payments.Commands.ProcessPaymentWithDecorators;

public sealed record ProcessPaymentWithDecoratorsCommand(
    Guid PaymentId,
    decimal Amount,
    string Currency,
    string Provider,
    IDictionary<string, string> PaymentDetails,
    bool ApplyFraudCheck = true,
    bool ApplyLimitCheck = true) : ICommand<PaymentResult>;

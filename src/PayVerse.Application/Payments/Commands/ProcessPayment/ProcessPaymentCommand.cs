using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Payments.Commands.ProcessPayment;

public sealed record ProcessPaymentCommand(
    Guid PaymentId,
    string ProviderName,
    IDictionary<string, string> PaymentDetails) : ICommand<PaymentDto>;

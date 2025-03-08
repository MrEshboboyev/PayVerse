using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Strategies.ValueObjects;

namespace PayVerse.Domain.Strategies.Payments;

/// <summary>
/// Defines the interface for all payment processing strategies
/// </summary>
public interface IPaymentProcessingStrategy
{
    Task<PaymentResult> ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken = default);
}
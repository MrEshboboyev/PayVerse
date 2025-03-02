using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Decorators.Payments;

/// <summary>
/// Defines the core contract for payment processing
/// </summary>
public interface IPaymentProcessor
{
    Task<PaymentResult> ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken = default);
}

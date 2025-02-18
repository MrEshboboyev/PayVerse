using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Infrastructure.Adapters;

/// <summary>
/// Defines the interface for processing payments through different payment gateways.
/// </summary>
public interface IPaymentGateway
{
    /// <summary>
    /// Processes a payment through the specific payment gateway.
    /// </summary>
    /// <param name="payment">The payment to process.</param>
    /// <returns>True if the payment was processed successfully, false otherwise.</returns>
    Task<bool> ProcessPaymentAsync(Payment payment);
}
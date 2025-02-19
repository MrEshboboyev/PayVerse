using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Shared;

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
    /// <returns>A Result object indicating the success or failure of the payment processing.</returns>
    Task<Result> ProcessPaymentAsync(Payment payment);
}

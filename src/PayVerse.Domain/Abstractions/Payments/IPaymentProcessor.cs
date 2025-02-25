using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.Abstractions.Payments;

/// <summary>
/// Represents a payment processor that handles payment operations.
/// </summary>
public interface IPaymentProcessor
{
    /// <summary>
    /// Processes a payment.
    /// </summary>
    /// <param name="payment">The payment to process.</param>
    /// <returns>Result of the payment processing.</returns>
    Task<Result> ProcessPaymentAsync(Payment payment);

    /// <summary>
    /// Validates a payment before processing.
    /// </summary>
    /// <param name="payment">The payment to validate.</param>
    /// <returns>Result of the validation.</returns>
    Task<Result> ValidatePaymentAsync(Payment payment);

    /// <summary>
    /// Cancels a payment.
    /// </summary>
    /// <param name="payment">The payment to cancel.</param>
    /// <returns>Result of the cancellation.</returns>
    Task<Result> CancelPaymentAsync(Payment payment);

    /// <summary>
    /// Refunds a payment.
    /// </summary>
    /// <param name="payment">The payment to refund.</param>
    /// <returns>Result of the refund.</returns>
    Task<Result> RefundPaymentAsync(Payment payment);
}
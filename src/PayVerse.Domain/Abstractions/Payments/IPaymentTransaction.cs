using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.Abstractions.Payments;

/// <summary>
/// Represents a payment transaction that records payment details.
/// </summary>
public interface IPaymentTransaction
{
    /// <summary>
    /// Gets the unique identifier for the transaction.
    /// </summary>
    string TransactionId { get; }

    /// <summary>
    /// Gets the timestamp when the transaction was created.
    /// </summary>
    DateTime Timestamp { get; }

    /// <summary>
    /// Creates a transaction record for a payment.
    /// </summary>
    /// <param name="payment">The payment for which to create a transaction.</param>
    /// <returns>Result containing the transaction details.</returns>
    Task<Result<string>> CreateTransactionAsync(Payment payment);

    /// <summary>
    /// Gets the status of a transaction.
    /// </summary>
    /// <param name="transactionId">The transaction identifier.</param>
    /// <returns>Result containing the transaction status.</returns>
    Task<Result<string>> GetTransactionStatusAsync(string transactionId);

    /// <summary>
    /// Generates a receipt for a transaction.
    /// </summary>
    /// <param name="transactionId">The transaction identifier.</param>
    /// <returns>Result containing the receipt.</returns>
    Task<Result<string>> GenerateReceiptAsync(string transactionId);
}
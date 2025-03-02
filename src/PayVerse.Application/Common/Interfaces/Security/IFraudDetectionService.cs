namespace PayVerse.Application.Common.Interfaces.Security;

/// <summary>
/// Service for detecting potentially fraudulent activities
/// </summary>
public interface IFraudDetectionService
{
    /// <summary>
    /// Analyzes a payment transaction for potential fraud
    /// </summary>
    /// <param name="paymentId">The ID of the payment to analyze</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if fraud is detected, otherwise false</returns>
    Task<bool> AnalyzePaymentAsync(Guid paymentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an account has suspicious login patterns
    /// </summary>
    /// <param name="userId">The user ID to check</param>
    /// <param name="ipAddress">The IP address of the login attempt</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A fraud risk score from 0 (no risk) to 100 (high risk)</returns>
    Task<int> CheckLoginPatternAsync(Guid userId, string ipAddress, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if a transaction amount is within normal usage patterns
    /// </summary>
    /// <param name="userId">The user ID making the transaction</param>
    /// <param name="amount">The transaction amount</param>
    /// <param name="currency">The transaction currency</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the transaction is within normal patterns, otherwise false</returns>
    Task<bool> ValidateTransactionAmountAsync(Guid userId, decimal amount, string currency, CancellationToken cancellationToken = default);
}
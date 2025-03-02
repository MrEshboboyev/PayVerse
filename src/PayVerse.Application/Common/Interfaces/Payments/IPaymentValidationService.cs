namespace PayVerse.Application.Common.Interfaces.Payments;

/// <summary>
/// Service for validating payment requests
/// </summary>
public interface IPaymentValidationService
{
    /// <summary>
    /// Validates a payment amount according to business rules
    /// </summary>
    /// <param name="amount">The payment amount</param>
    /// <param name="currency">The payment currency</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the payment is valid, otherwise false</returns>
    Task<bool> ValidatePaymentAmountAsync(decimal amount, string currency, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if a user has sufficient balance to make a payment
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="amount">The payment amount</param>
    /// <param name="currency">The payment currency</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the user has sufficient balance, otherwise false</returns>
    Task<bool> ValidateUserBalanceAsync(Guid userId, decimal amount, string currency, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if a payment method is accepted and valid
    /// </summary>
    /// <param name="paymentMethodId">The payment method ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the payment method is valid, otherwise false</returns>
    Task<bool> ValidatePaymentMethodAsync(string paymentMethodId, CancellationToken cancellationToken = default);
}

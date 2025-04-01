using Microsoft.Extensions.Logging;
using PayVerse.Application.Common.Interfaces.Payments;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Repositories.Wallets;

namespace PayVerse.Infrastructure.Services.Payments;

/// <summary>
/// Implementation of payment validation service
/// </summary>
public class PaymentValidationService(
    IVirtualAccountRepository virtualAccountRepository,
    ILogger<PaymentValidationService> logger) : IPaymentValidationService
{
    public Task<bool> ValidatePaymentAmountAsync(decimal amount, string currency, CancellationToken cancellationToken = default)
    {
        // Implement business rules for validating payment amounts

        // Example implementation
        if (amount <= 0)
        {
            logger.LogWarning("Invalid payment amount: {Amount} {Currency}", amount, currency);
            return Task.FromResult(false);
        }

        // Check for maximum payment limits
        var maximumAmount = GetMaximumAmount(currency);
        if (amount > maximumAmount)
        {
            logger.LogWarning("Payment amount exceeds maximum limit: {Amount} {Currency}", amount, currency);
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }

    public async Task<bool> ValidateUserBalanceAsync(Guid userId, decimal amount, string currency, CancellationToken cancellationToken = default)
    {
        // Get user's virtual account with matching currency
        var accounts = await virtualAccountRepository.GetAllAsync(cancellationToken);
        var account = accounts.FirstOrDefault(a => a.UserId == userId && a.Currency.Code == currency);

        if (account == null)
        {
            logger.LogWarning("User {UserId} has no virtual account in {Currency}", userId, currency);
            return false;
        }

        // Check if account has sufficient balance
        if (account.Balance.Value < amount)
        {
            logger.LogInformation("Insufficient balance for user {UserId}: {Balance} {Currency}, required: {Amount}",
                userId, account.Balance.Value, currency, amount);
            return false;
        }

        return true;
    }

    public Task<bool> ValidatePaymentMethodAsync(string paymentMethodId, CancellationToken cancellationToken = default)
    {
        // Implement validation of payment methods
        // This would typically involve checking against stored payment methods or
        // validating with a payment provider

        // Example implementation
        if (string.IsNullOrEmpty(paymentMethodId))
        {
            logger.LogWarning("Invalid payment method ID: null or empty");
            return Task.FromResult(false);
        }

        // Additional validation logic would go here

        return Task.FromResult(true);
    }

    private decimal GetMaximumAmount(string currency)
    {
        // Example implementation - would typically be configured based on currency or other factors
        return currency switch
        {
            "USD" => 10000m,
            "EUR" => 9000m,
            _ => 5000m
        };
    }
}
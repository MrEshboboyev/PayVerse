// Abstract interface for payment operations
using PayVerse.Application.Common.Interfaces;

namespace PayVerse.Infrastructure.Security.Proxies;

// Proxy implementation with additional security and logging
public class PaymentServiceProxy(
    RealPaymentService realPaymentService,
    ISecurityService securityService,
    ILoggingService loggingService) : IPaymentService
{
    public async Task<bool> ProcessPaymentAsync(Guid userId, decimal amount)
    {
        // Logging before the operation
        loggingService.LogInfo($"Attempt to process payment for user {userId}");

        // Security check before processing payment
        if (!await securityService.IsUserAuthorizedAsync(userId))
        {
            loggingService.LogWarning($"Unauthorized payment attempt by user {userId}");
            throw new UnauthorizedAccessException("User not authorized to process payment");
        }

        // Additional security checks
        if (amount <= 0)
        {
            loggingService.LogError($"Invalid payment amount {amount}");
            throw new ArgumentException("Payment amount must be positive");
        }

        // Check for suspicious activity
        if (await securityService.IsSuspiciousTransactionAsync(userId, amount))
        {
            loggingService.LogWarning($"Suspicious transaction detected for user {userId}");
            await securityService.FlagAccountAsync(userId);
            return false;
        }

        // Proceed with actual payment
        bool result = await realPaymentService.ProcessPaymentAsync(userId, amount);

        // Logging after the operation
        if (result)
        {
            loggingService.LogInfo($"Payment processed successfully for user {userId}");
        }
        else
        {
            loggingService.LogError($"Payment processing failed for user {userId}");
        }

        return result;
    }

    public async Task<decimal> GetAccountBalanceAsync(Guid userId)
    {
        // Logging before the operation
        loggingService.LogInfo($"Balance inquiry for user {userId}");

        // Security check before retrieving balance
        if (!await securityService.IsUserAuthorizedAsync(userId))
        {
            loggingService.LogWarning($"Unauthorized balance inquiry by user {userId}");
            throw new UnauthorizedAccessException("User not authorized to view balance");
        }

        // Proceed with balance retrieval
        decimal balance = await realPaymentService.GetAccountBalanceAsync(userId);

        // Logging after the operation
        loggingService.LogInfo($"Balance retrieved successfully for user {userId}");

        return balance;
    }
}
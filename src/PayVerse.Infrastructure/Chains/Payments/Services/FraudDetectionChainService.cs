using Microsoft.EntityFrameworkCore;
using PayVerse.Domain.Chains.Payments.Models;
using PayVerse.Domain.Chains.Payments.Services;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Persistence;

namespace PayVerse.Infrastructure.Chains.Payments.Services;

/// <summary>
/// Fraud Detection Chain Service implementations
/// </summary>
public class FraudDetectionChainService(ApplicationDbContext context) : IFraudDetectionChainService
{
    private const int SUSPICIOUS_TRANSACTION_THRESHOLD = 3;
    private const decimal UNUSUAL_TRANSACTION_AMOUNT = 5000m;
    private const int DAYS_TO_CHECK = 30;

    public async Task<FraudCheckChainResult> CheckPaymentForFraud(Payment payment)
    {
        // Check multiple fraud indicators
        bool isSuspicious = await CheckRecentTransactionFrequency(payment)
                          || CheckUnusualTransactionAmount(payment)
                          || await CheckGeographicalInconsistency(payment);

        return new FraudCheckChainResult
        {
            IsSuspicious = isSuspicious
        };
    }

    private async Task<bool> CheckRecentTransactionFrequency(Payment payment)
    {
        // Check for unusually high number of transactions in recent period
        var recentTransactionsCount = await context.Set<Payment>()
            .Where(p => p.UserId == payment.UserId &&
                        p.CreatedOnUtc >= DateTime.UtcNow.AddDays(-DAYS_TO_CHECK))
            .CountAsync();

        return recentTransactionsCount > SUSPICIOUS_TRANSACTION_THRESHOLD;
    }

    private bool CheckUnusualTransactionAmount(Payment payment)
    {
        // Flag transactions significantly larger than typical
        return payment.Amount.Value > UNUSUAL_TRANSACTION_AMOUNT;
    }

    private async Task<bool> CheckGeographicalInconsistency(Payment payment)
    {
        // Check for transactions from unusual locations
        var recentPayments = await context.Set<Payment>()
            .Where(p => p.UserId == payment.UserId &&
                        p.CreatedOnUtc >= DateTime.UtcNow.AddDays(-DAYS_TO_CHECK))
            .ToListAsync();

        // In a real-world scenario, this would involve more complex 
        // geographical and IP-based checks
        return recentPayments.Count > 0 &&
               recentPayments.Any(p => p.ProviderName != payment.ProviderName);
    }
}

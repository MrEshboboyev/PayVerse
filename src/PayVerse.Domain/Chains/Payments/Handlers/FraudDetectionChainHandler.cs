using PayVerse.Domain.Chains.Payments.Models;
using PayVerse.Domain.Chains.Payments.Services;
using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Chains.Payments.Handlers;

/// <summary>
/// Performs fraud detection checks in the processing chain
/// </summary>
public class FraudDetectionChainHandler(
    IFraudDetectionChainService fraudDetectionChainService) : PaymentProcessingChainHandler
{
    public override async Task<PaymentProcessingChainResult> ProcessPaymentInChain(Payment payment)
    {
        // Perform fraud detection checks
        var fraudCheckResult = await fraudDetectionChainService.CheckPaymentForFraud(payment);

        if (fraudCheckResult.IsSuspicious)
        {
            return new PaymentProcessingChainResult
            {
                IsSuccessful = false,
                ErrorMessage = "Payment flagged for suspicious activity"
            };
        }

        // If no issues, pass to next handler
        return _nextChainHandler != null
            ? await _nextChainHandler.ProcessPaymentInChain(payment)
            : new PaymentProcessingChainResult { IsSuccessful = true };
    }
}

using PayVerse.Domain.Chains.Payments.Models;
using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Chains.Payments.Handlers;

/// <summary>
/// Validates payment amount constraints in the processing chain
/// </summary>
public class PaymentAmountValidationChainHandler : PaymentProcessingChainHandler
{
    private const decimal MaxDailyChainLimit = 10000m;
    private const decimal MinPaymentChainAmount = 1m;

    public override async Task<PaymentProcessingChainResult> ProcessPaymentInChain(Payment payment)
    {
        // Validate payment amount
        if (payment.Amount.Value < MinPaymentChainAmount)
        {
            return new PaymentProcessingChainResult
            {
                IsSuccessful = false,
                ErrorMessage = $"Payment amount must be at least {MinPaymentChainAmount}"
            };
        }

        // Check daily limit (would typically involve checking total payments for the day)
        if (payment.Amount.Value > MaxDailyChainLimit)
        {
            return new PaymentProcessingChainResult
            {
                IsSuccessful = false,
                ErrorMessage = $"Daily payment limit of {MaxDailyChainLimit} exceeded"
            };
        }

        // If no issues, pass to next handler
        return _nextChainHandler != null
            ? await _nextChainHandler.ProcessPaymentInChain(payment)
            : new PaymentProcessingChainResult { IsSuccessful = true };
    }
}
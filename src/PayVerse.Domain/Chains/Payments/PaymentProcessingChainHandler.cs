using PayVerse.Domain.Chains.Payments.Models;
using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Chains.Payments;


/// <summary>
/// Abstract base class for payment processing chain handlers
/// </summary>
public abstract class PaymentProcessingChainHandler
{
    // Next handler in the chain
    protected PaymentProcessingChainHandler _nextChainHandler;

    /// <summary>
    /// Set the next handler in the chain
    /// </summary>
    public PaymentProcessingChainHandler SetNextChainHandler(PaymentProcessingChainHandler handler)
    {
        _nextChainHandler = handler;
        return handler;
    }

    /// <summary>
    /// Abstract method to process the payment in the chain
    /// </summary>
    public abstract Task<PaymentProcessingChainResult> ProcessPaymentInChain(Payment payment);
}
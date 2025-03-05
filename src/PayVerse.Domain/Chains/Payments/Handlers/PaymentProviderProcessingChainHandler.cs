using PayVerse.Domain.Chains.Payments.Models;
using PayVerse.Domain.Chains.Payments.Services;
using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Chains.Payments.Handlers;

/// <summary>
/// Final handler to process the payment through a payment provider in the chain
/// </summary>
public class PaymentProviderProcessingChainHandler(IPaymentProviderChainService paymentProviderChainService) : PaymentProcessingChainHandler
{
    public override async Task<PaymentProcessingChainResult> ProcessPaymentInChain(Payment payment)
    {
        try
        {
            // Process payment through external payment provider
            var providerResult = await paymentProviderChainService.ProcessPayment(payment);

            return new PaymentProcessingChainResult
            {
                IsSuccessful = providerResult.IsSuccessful,
                ErrorMessage = providerResult.ErrorMessage
            };
        }
        catch (Exception ex)
        {
            return new PaymentProcessingChainResult
            {
                IsSuccessful = false,
                ErrorMessage = $"Payment processing failed: {ex.Message}"
            };
        }
    }
}

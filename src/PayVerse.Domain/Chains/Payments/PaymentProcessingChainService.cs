using PayVerse.Domain.Chains.Payments.Handlers;
using PayVerse.Domain.Chains.Payments.Models;
using PayVerse.Domain.Chains.Payments.Services;
using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Chains.Payments;

/// <summary>
/// Payment processing service that sets up the chain of responsibility
/// </summary>
public class PaymentProcessingChainService
{
    private readonly PaymentProcessingChainHandler _processingChain;

    public PaymentProcessingChainService(
        IVirtualAccountChainService virtualAccountChainService,
        IFraudDetectionChainService fraudDetectionChainService,
        IPaymentProviderChainService paymentProviderChainService)
    {
        // Construct the processing chain
        var amountValidationChainHandler = new PaymentAmountValidationChainHandler();
        var balanceValidationChainHandler = new AccountBalanceValidationChainHandler(virtualAccountChainService);
        var fraudDetectionChainHandler = new FraudDetectionChainHandler(fraudDetectionChainService);
        var providerProcessingChainHandler = new PaymentProviderProcessingChainHandler(paymentProviderChainService);

        // Link the chain
        amountValidationChainHandler
            .SetNextChainHandler(balanceValidationChainHandler)
            .SetNextChainHandler(fraudDetectionChainHandler)
            .SetNextChainHandler(providerProcessingChainHandler);

        _processingChain = amountValidationChainHandler;
    }

    public async Task<PaymentProcessingChainResult> ProcessPaymentInChain(Payment payment)
    {
        return await _processingChain.ProcessPaymentInChain(payment);
    }
}

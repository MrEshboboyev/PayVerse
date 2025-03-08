using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Strategies.Factories;
using PayVerse.Domain.Strategies.ValueObjects;

namespace PayVerse.Domain.Strategies.Services;

/// <summary>
/// Context class that uses payment processing strategies
/// </summary>
public class PaymentProcessor(IPaymentProcessingStrategyFactory strategyFactory)
{
    public async Task<PaymentResult> ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        // Get the appropriate strategy based on the payment method
        var strategy = strategyFactory.CreateStrategy(payment.PaymentMethodEntity.Type);

        // Use the strategy to process the payment
        var result = await strategy.ProcessPaymentAsync(payment, cancellationToken);

        return result;
    }
}
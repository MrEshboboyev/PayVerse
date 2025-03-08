using Microsoft.Extensions.DependencyInjection;
using PayVerse.Domain.Strategies.Payments;
using PayVerse.Domain.Strategies.ValueObjects;

namespace PayVerse.Domain.Strategies.Factories;

/// <summary>
/// Concrete factory implementation for creating payment processing strategies
/// </summary>
public class PaymentProcessingStrategyFactory(
    IServiceProvider serviceProvider) : IPaymentProcessingStrategyFactory
{
    public IPaymentProcessingStrategy CreateStrategy(PaymentMethodType paymentMethodType)
    {
        return paymentMethodType switch
        {
            PaymentMethodType.Wallet => serviceProvider.GetRequiredService<WalletPaymentStrategy>(),
            _ => throw new NotSupportedException($"Payment method type {paymentMethodType} is not supported.")
        };
    }
}
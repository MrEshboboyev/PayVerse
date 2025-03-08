using PayVerse.Domain.Strategies.Payments;
using PayVerse.Domain.Strategies.ValueObjects;

namespace PayVerse.Domain.Strategies.Factories;

/// <summary>
/// Factory interface for creating payment processing strategies
/// </summary>
public interface IPaymentProcessingStrategyFactory
{
    IPaymentProcessingStrategy CreateStrategy(PaymentMethodType paymentMethodType);
}

using PayVerse.Domain.Decorators.Payments;
using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Application.Payments.Decorators;

/// <summary>
/// Abstract base decorator for payment processors
/// </summary>
public abstract class PaymentProcessorDecorator : IPaymentProcessor
{
    protected readonly IPaymentProcessor _decorated;

    protected PaymentProcessorDecorator(IPaymentProcessor paymentProcessor)
    {
        _decorated = paymentProcessor;
    }

    public virtual async Task<PaymentResult> ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        // By default, delegate to the decorated component
        return await _decorated.ProcessPaymentAsync(payment, cancellationToken);
    }
}

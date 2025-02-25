using PayVerse.Domain.Abstractions.Payments;
using Microsoft.Extensions.Options;
using PayVerse.Infrastructure.Payments.Stripe.Models;

namespace PayVerse.Infrastructure.Payments.Stripe;

/// <summary>
/// Stripe implementation of the payment provider factory.
/// </summary>
internal sealed class StripeFactory(IOptions<StripeSettings> settings) : IPaymentProviderFactory
{
    public string ProviderName => "Stripe";

    public IPaymentProcessor CreatePaymentProcessor()
    {
        return new StripePaymentProcessor(settings);
    }

    public IPaymentTransaction CreateTransactionManager()
    {
        return new StripeTransactionManager(settings);
    }
}
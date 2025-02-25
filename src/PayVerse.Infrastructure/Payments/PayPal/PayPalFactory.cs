using PayVerse.Domain.Abstractions.Payments;
using PayVerse.Infrastructure.Payments.PayPal.Models;
using Microsoft.Extensions.Options;

namespace PayVerse.Infrastructure.Payments.PayPal;

/// <summary>
/// PayPal implementation of the payment provider factory.
/// </summary>
internal sealed class PayPalFactory(
    IHttpClientFactory httpClientFactory,
    IOptions<PayPalSettings> settings) : IPaymentProviderFactory
{
    public string ProviderName => "PayPal";

    public IPaymentProcessor CreatePaymentProcessor()
    {
        var client = httpClientFactory.CreateClient("PayPal");
        return new PayPalPaymentProcessor(client, settings);
    }

    public IPaymentTransaction CreateTransactionManager()
    {
        var client = httpClientFactory.CreateClient("PayPal");
        return new PayPalTransactionManager(client, settings);
    }
}

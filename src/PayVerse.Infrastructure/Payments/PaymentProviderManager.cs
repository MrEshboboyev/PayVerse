using Microsoft.Extensions.Options;
using PayVerse.Domain.Abstractions.Payments;
using PayVerse.Infrastructure.Payments.Models;

namespace PayVerse.Infrastructure.Payments;

/// <summary>
/// Implements the payment provider manager.
/// </summary>
internal sealed class PaymentProviderManager(
    IEnumerable<IPaymentProviderFactory> factories,
    IOptions<PaymentSettings> settings) : IPaymentProviderManager
{
    private readonly Dictionary<string, IPaymentProviderFactory> _factories =
        factories.ToDictionary(f => f.ProviderName.ToLower(), f => f);
    private readonly string _defaultProvider = 
        settings.Value.DefaultProvider.ToLower();

    public IPaymentProviderFactory GetFactory(string providerName)
    {
        if (string.IsNullOrEmpty(providerName))
        {
            return GetDefaultFactory();
        }

        var key = providerName.ToLower();

        if (_factories.TryGetValue(key, out var factory))
        {
            return factory;
        }

        throw new KeyNotFoundException($"Payment provider '{providerName}' not found.");
    }

    public IPaymentProviderFactory GetDefaultFactory()
    {
        if (_factories.TryGetValue(_defaultProvider, out var factory))
        {
            return factory;
        }

        throw new InvalidOperationException($"Default payment provider '{_defaultProvider}' not found.");
    }
}
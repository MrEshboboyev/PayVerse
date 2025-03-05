using PayVerse.Domain.Errors;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;
using System.Collections.Concurrent;

namespace PayVerse.Domain.Flyweights;

/// <summary>
/// Flyweight factory for managing Currency instances
/// </summary>
public class CurrencyFlyweight
{
    // Singleton thread-safe implementation
    private static readonly Lazy<CurrencyFlyweight> _instance =
        new(() => new CurrencyFlyweight());

    public static CurrencyFlyweight Instance => _instance.Value;

    // Concurrent dictionary for thread-safe caching of successful Currency results
    private ConcurrentDictionary<string, Result<Currency>> _currencyCache = new();

    private CurrencyFlyweight()
    {
        // Preload common currencies
        PreloadCommonCurrencies();
    }

    private void PreloadCommonCurrencies()
    {
        // Preload most common currencies
        GetCurrency("USD");
        GetCurrency("EUR");
        GetCurrency("GBP");
        GetCurrency("JPY");
        GetCurrency("CHF");
    }

    /// <summary>
    /// Get or create a Currency instance with flyweight optimization
    /// </summary>
    public Result<Currency> GetCurrency(string code)
    {
        return _currencyCache.GetOrAdd(code.ToUpper(), CreateCurrency);
    }

    /// <summary>
    /// Internal method to create Currency, using the existing factory method
    /// </summary>
    private Result<Currency> CreateCurrency(string code)
    {
        return Currency.Create(code);
    }

    /// <summary>
    /// Try to get an existing currency by its code
    /// </summary>
    public Result<Currency> TryGetCurrency(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Result.Failure<Currency>(DomainErrors.Currency.Invalid);
        }

        return _currencyCache.TryGetValue(code.ToUpper(), out var currency)
            ? currency
            : Result.Failure<Currency>(DomainErrors.Currency.Invalid);
    }

    /// <summary>
    /// Clear the currency cache (useful for testing or cache management)
    /// </summary>
    internal void ClearCache()
    {
        _currencyCache.Clear();
        PreloadCommonCurrencies();
    }
}

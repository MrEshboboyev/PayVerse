using PayVerse.Domain.Flyweights;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Domain.Extensions;

public static class CurrencyExtensions
{
    /// <summary>
    /// Convenient extension method to get Currency from Flyweight
    /// </summary>
    public static Result<Currency> GetCurrency(this string currencyCode)
    {
        return CurrencyFlyweight.Instance.GetCurrency(currencyCode);
    }
}
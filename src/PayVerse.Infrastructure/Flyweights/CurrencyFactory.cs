using Microsoft.Extensions.Logging;
using PayVerse.Domain.ValueObjects;
using System.Collections.Concurrent;

namespace PayVerse.Infrastructure.Flyweights;

public class CurrencyFactory
{
    private static readonly ConcurrentDictionary<string, Currency> _currencies = 
        new(StringComparer.OrdinalIgnoreCase);
    private static readonly ILogger<CurrencyFactory> _logger;

    static CurrencyFactory()
    {
        // Assuming you have dependency injection set up for logging
        _logger = LoggerFactory.Create(builder => { }).CreateLogger<CurrencyFactory>();
    }

    public static Currency GetCurrency(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Currency code cannot be null or empty.", nameof(code));
        }

        return _currencies.GetOrAdd(code, CreateCurrency);

        static Currency CreateCurrency(string code)
        {
            try
            {
                var result = Currency.Create(code);
                if (result.IsFailure)
                {
                    throw new InvalidOperationException($"Failed to create currency for {code}: {result.Error}");
                }
                string message = $"Creating new currency object for {code}";
                _logger.LogInformation(message);
                return result.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating currency for {code}");
                throw;
            }
        }
    }
}
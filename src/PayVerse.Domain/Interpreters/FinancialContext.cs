namespace PayVerse.Domain.Interpreters;

public class FinancialContext(
    Dictionary<string, decimal> exchangeRates, 
    DateTime date)
{
    private readonly Dictionary<string, decimal> _exchangeRates = 
        exchangeRates ?? throw new ArgumentNullException(nameof(exchangeRates));
    private readonly DateTime _date = date;

    public decimal GetExchangeRate(string currencyCode)
    {
        if (_exchangeRates.TryGetValue(currencyCode, out var rate))
        {
            return rate;
        }
        throw new InvalidOperationException($"No exchange rate available for {currencyCode}");
    }

    public DateTime GetDate() => _date;

    // Additional methods could include:
    // - Getting tax rates, inflation rates, etc.
    // - Handling more complex financial calculations based on date or other factors
}
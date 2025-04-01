using PayVerse.Domain.Shared;
using PayVerse.Application.Wallets.Converters;

namespace PayVerse.Infrastructure.Converters;

public class CurrencyConverter() : ICurrencyConverter
{
    public async Task<Result<decimal>> ConvertAsync(
        decimal amount,
        string currencyCode,
        string targetCurrencyCode,
        CancellationToken cancellationToken = default)
    {

        await Task.Delay(100); // Simulate async operation

        // try
        // {
        //     // Logic to call external currency conversion API and calculate the conversion
        //     var response = await httpClient
        //         .GetAsync($"https://api.exchangerate-api.com/v4/latest/{currencyCode}",
        //         cancellationToken);
        //     response.EnsureSuccessStatusCode();
        //
        //     var exchangeRates = await response.Content.ReadAsAsync<ExchangeRateResponse>(cancellationToken);
        //     var conversionRate = exchangeRates.Rates[targetCurrencyCode];
        //     var convertedAmount = amount * conversionRate;
        //
        //     return Result.Success(convertedAmount);
        // }
        // catch (Exception ex)
        // {
        //     return Result.Failure<decimal>(ex.Message);
        // }

        return Result.Success(10m);
    }
    //
    // private class ExchangeRateResponse
    // {
    //     public Dictionary<string, decimal> Rates { get; set; }
    // }
}
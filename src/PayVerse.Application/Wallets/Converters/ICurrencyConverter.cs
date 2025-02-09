using PayVerse.Domain.Shared;

namespace PayVerse.Application.Wallets.Converters;

public interface ICurrencyConverter
{
    Task<Result<decimal>> ConvertAsync(
        decimal amount,
        string currencyCode,
        string targetCurrencyCode,
        CancellationToken cancellationToken = default);
}
using Microsoft.Extensions.Logging;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Shared;
using PayVerse.Domain.Strategies;

namespace PayVerse.Infrastructure.Strategies;

public class CryptoPaymentStrategy(ILogger<CryptoPaymentStrategy> logger) : IPaymentStrategy
{
    private readonly ILogger<CryptoPaymentStrategy> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result> ProcessPayment(Payment payment)
    {
        try
        {
            // Simulate crypto payment processing
            await Task.Delay(1000); // Simulating some network latency

            _logger.LogInformation($"₿ Processing Crypto Payment for {payment.Id}: {payment.Amount.Value} {payment.Currency}");

            // Here you would have actual integration with a crypto payment gateway
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing Crypto Payment for {payment.Id}");
            return Result.Failure(
                DomainErrors.Payment.CryptoProcessingFailed(payment.Id, ex.Message));
        }
    }
}

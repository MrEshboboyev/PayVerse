using Microsoft.Extensions.Logging;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Shared;
using PayVerse.Domain.Strategies;

namespace PayVerse.Infrastructure.Strategies;

public class CreditCardPaymentStrategy(ILogger<CreditCardPaymentStrategy> logger) : IPaymentStrategy
{
    private readonly ILogger<CreditCardPaymentStrategy> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result> ProcessPayment(Payment payment)
    {
        try
        {
            // Simulate credit card payment processing
            await Task.Delay(500); // Simulating some processing time

            _logger.LogInformation($"💳 Processing Credit Card Payment for {payment.Id}: {payment.Amount.Value} USD");

            // Here you would integrate with a credit card payment gateway like Stripe or PayPal
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing Credit Card Payment for {payment.Id}");
            return Result.Failure(
                DomainErrors.Payment.CreditCardProcessingFailed(payment.Id, ex.Message));
        }
    }
}

using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Shared;
using Stripe;

namespace PayVerse.Infrastructure.Adapters;

public class StripeAdapter : IPaymentGateway
{
    public async Task<Result> ProcessPaymentAsync(Payment payment)
    {
        try
        {
            // Initialize Stripe with API keys (typically done in the constructor or config)
            StripeConfiguration.ApiKey = "your_stripe_api_key";

            var options = new ChargeCreateOptions
            {
                Amount = (long)(payment.Amount.Value * 100), // Convert to cents for Stripe
                Currency = "usd",
                Description = "Payment for invoice",
                // Other Stripe charge options would go here
            };

            var service = new ChargeService();
            var charge = await service.CreateAsync(options);

            Console.WriteLine($"Processing payment {payment.Id} of ${payment.Amount.Value} via Stripe...");
            if (charge.Status == "succeeded")
            {
                return Result.Success();
            }

            return Result.Failure(DomainErrors.Stripe.ProcessingFailed("Payment processing failed."));
        }
        catch (StripeException e)
        {
            Console.WriteLine($"Stripe payment failed: {e.Message}");
            return Result.Failure(DomainErrors.Stripe.ProcessingFailed(e.Message));
        }
    }
}

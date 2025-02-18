using PayVerse.Domain.Entities.Payments;
using Stripe;
using System;

namespace PayVerse.Infrastructure.Adapters;

public class StripeAdapter : IPaymentGateway
{
    public async Task<bool> ProcessPaymentAsync(Payment payment)
    {
        try
        {
            // Here we would typically initialize Stripe with API keys.
            var options = new ChargeCreateOptions
            {
                Amount = (long)(payment.Amount.Value * 100), // Convert to cents for Stripe
                Currency = "usd",
                // Other Stripe charge options would go here
            };

            var service = new ChargeService();
            var charge = service.Create(options);

            Console.WriteLine($"Processing payment {payment.Id} of ${payment.Amount} via Stripe...");
            return charge.Status == "succeeded";
        }
        catch (StripeException e)
        {
            Console.WriteLine($"Stripe payment failed: {e.Message}");
            return false;
        }
    }
}
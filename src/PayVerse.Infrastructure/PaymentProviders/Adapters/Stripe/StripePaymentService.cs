namespace PayVerse.Infrastructure.PaymentProviders.Adapters.Stripe;

// Example of an external payment provider with an incompatible interface
public class StripePaymentService
{
    public async Task<StripeChargeResult> CreateChargeAsync(string customerId,
                                                            decimal amount,
                                                            string currency,
                                                            string description)
    {
        // This would call the actual Stripe API in a real implementation
        await Task.Delay(100); // Simulate API call

        return new StripeChargeResult
        {
            Success = true,
            ChargeId = $"ch_{Guid.NewGuid():N}",
            Created = DateTime.UtcNow
        };
    }

    public async Task<StripeRefundResult> CreateRefundAsync(string chargeId,
                                                            decimal amount)
    {
        // This would call the actual Stripe API in a real implementation
        await Task.Delay(100); // Simulate API call

        return new StripeRefundResult
        {
            Success = true,
            RefundId = $"re_{Guid.NewGuid():N}",
            Created = DateTime.UtcNow
        };
    }

    public async Task<StripeChargeStatus> RetrieveChargeAsync(string chargeId)
    {
        // This would call the actual Stripe API in a real implementation
        await Task.Delay(100); // Simulate API call

        return new StripeChargeStatus
        {
            Status = "succeeded",
            Amount = 1000,
            Created = DateTime.UtcNow.AddMinutes(-5)
        };
    }
}

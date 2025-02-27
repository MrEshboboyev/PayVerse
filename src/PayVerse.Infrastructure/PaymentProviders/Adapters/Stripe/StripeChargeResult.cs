namespace PayVerse.Infrastructure.PaymentProviders.Adapters.Stripe;

// Stripe specific result classes
public class StripeChargeResult
{
    public bool Success { get; set; }
    public string ChargeId { get; set; }
    public DateTime Created { get; set; }
    public string FailureMessage { get; set; }
}

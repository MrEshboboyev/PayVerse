namespace PayVerse.Infrastructure.PaymentProviders.Adapters.Stripe;

public class StripeChargeStatus
{
    public string Status { get; set; }
    public decimal Amount { get; set; }
    public DateTime Created { get; set; }
}

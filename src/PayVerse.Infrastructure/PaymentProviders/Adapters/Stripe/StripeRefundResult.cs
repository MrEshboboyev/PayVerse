namespace PayVerse.Infrastructure.PaymentProviders.Adapters.Stripe;

public class StripeRefundResult
{
    public bool Success { get; set; }
    public string RefundId { get; set; }
    public DateTime Created { get; set; }
    public string FailureMessage { get; set; }
}

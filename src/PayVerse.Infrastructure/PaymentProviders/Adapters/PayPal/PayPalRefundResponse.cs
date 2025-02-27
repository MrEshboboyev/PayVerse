namespace PayVerse.Infrastructure.PaymentProviders.Adapters.PayPal;

public class PayPalRefundResponse
{
    public string RefundId { get; set; }
    public string Status { get; set; }
    public DateTime Timestamp { get; set; }
    public string ErrorMessage { get; set; }
}

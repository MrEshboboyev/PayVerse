namespace PayVerse.Infrastructure.PaymentProviders.Adapters.PayPal;

public class PayPalPaymentStatus
{
    public string Status { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
}

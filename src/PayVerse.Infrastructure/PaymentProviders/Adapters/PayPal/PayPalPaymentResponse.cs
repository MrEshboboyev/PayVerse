namespace PayVerse.Infrastructure.PaymentProviders.Adapters.PayPal;

public class PayPalPaymentResponse
{
    public string PaymentId { get; set; }
    public string Status { get; set; }
    public DateTime Timestamp { get; set; }
    public string ErrorMessage { get; set; }
}

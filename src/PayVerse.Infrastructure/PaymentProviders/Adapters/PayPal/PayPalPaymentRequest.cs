namespace PayVerse.Infrastructure.PaymentProviders.Adapters.PayPal;

// PayPal specific classes
public class PayPalPaymentRequest
{
    public string CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Description { get; set; }
}

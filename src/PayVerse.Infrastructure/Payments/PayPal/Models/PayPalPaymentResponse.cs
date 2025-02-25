namespace PayVerse.Infrastructure.Payments.PayPal.Models;

public class PayPalPaymentResponse
{
    public string Id { get; set; }
    public string Status { get; set; }
    public PayPalLink[] Links { get; set; }
}

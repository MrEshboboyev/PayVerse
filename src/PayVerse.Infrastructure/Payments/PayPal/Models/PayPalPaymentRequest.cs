namespace PayVerse.Infrastructure.Payments.PayPal.Models;

public class PayPalPaymentRequest
{
    public string Intent { get; set; }
    public PurchaseUnit[] PurchaseUnits { get; set; }
}

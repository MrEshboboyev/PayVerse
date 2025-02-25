namespace PayVerse.Infrastructure.Payments.PayPal.Models;

public class PayPalRefundRequest
{
    public PayPalAmount Amount { get; set; }
    public string NoteToPayer { get; set; }
}
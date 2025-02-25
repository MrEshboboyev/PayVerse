namespace PayVerse.Infrastructure.Payments.PayPal.Models;

public class PurchaseUnit
{
    public PayPalAmount Amount { get; set; }
    public string ReferenceId { get; set; }
}

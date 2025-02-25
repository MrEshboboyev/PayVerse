namespace PayVerse.Infrastructure.Payments.PayPal.Models;

public class PayPalTokenResponse
{
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
}

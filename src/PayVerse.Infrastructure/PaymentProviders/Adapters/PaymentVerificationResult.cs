using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Infrastructure.PaymentProviders.Adapters;

public class PaymentVerificationResult
{
    public bool IsVerified { get; set; }
    public PaymentStatus Status { get; set; }
    public string ErrorMessage { get; set; }
}
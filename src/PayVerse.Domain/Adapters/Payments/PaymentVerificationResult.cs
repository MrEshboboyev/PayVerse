using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Domain.Adapters.Payments;

public class PaymentVerificationResult
{
    public bool IsVerified { get; set; }
    public PaymentStatus Status { get; set; }
    public string ErrorMessage { get; set; }
}
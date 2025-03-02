namespace PayVerse.Domain.Adapters.Payments;

// Result classes
public class PaymentProcessResult
{
    public bool IsSuccess { get; set; }
    public string TransactionId { get; set; }
    public string ErrorMessage { get; set; }
    public DateTime ProcessedDate { get; set; }
}

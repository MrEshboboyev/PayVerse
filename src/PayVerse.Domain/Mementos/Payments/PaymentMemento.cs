using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Mementos.Payments;

/// <summary>
/// Concrete memento class for storing Payment state
/// </summary>
public class PaymentMemento : IMemento
{
    private readonly string _state;
    private readonly string _metadata;
    private readonly DateTime _createdAt;

    public DateTime CreatedAt => _createdAt;

    public PaymentMemento(Payment payment, string metadata = "")
    {
        _state = System.Text.Json.JsonSerializer.Serialize(new
        {
            payment.Id,
            Amount = payment.Amount.Value,
            Currency = "USD",
            payment.Status,
            payment.UserId,
            payment.ScheduledDate,
            payment.TransactionId,
            payment.RefundTransactionId,
            payment.ProviderName,
            payment.ProcessedDate,
            payment.RefundedDate,
            payment.CancelledDate,
            payment.FailureReason,
            PaymentMethod = payment.PaymentMethod?.ToString(),
            payment.CreatedOnUtc,
            payment.ModifiedOnUtc
        });

        _metadata = metadata;
        _createdAt = DateTime.UtcNow;
    }

    public string GetState() => _state;
    public string GetMetadata() => _metadata;
}
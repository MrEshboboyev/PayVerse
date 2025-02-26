namespace PayVerse.Domain.Enums.Payments;

public enum PaymentStatus
{
    Pending = 10,
    Processed = 15,
    Scheduled = 17,
    Completed = 20,
    Failed = 30,
    Cancelled = 40,
    Refunded = 50
}
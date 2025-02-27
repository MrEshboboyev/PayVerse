namespace PayVerse.Domain.Enums.Payments;

public enum PaymentStatus
{
    Pending = 10,
    Processing = 12,
    Processed = 15,
    Scheduled = 17,
    Completed = 20,
    Failed = 30,
    Cancelled = 40,
    Refunded = 50,
    Unknown = 60
}
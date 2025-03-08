namespace PayVerse.Domain.Strategies.ValueObjects;

/// <summary>
/// Represents the result of a payment processing operation
/// </summary>
public class PaymentResult(bool success, string transactionId, string failureReason)
{
    public bool Success { get; } = success;
    public string TransactionId { get; } = transactionId;
    public string FailureReason { get; } = failureReason;
}

/// <summary>
/// Enum for payment method types
/// </summary>
public enum PaymentMethodType
{
    Wallet
}
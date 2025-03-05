namespace PayVerse.Domain.Chains.Payments.Models;

/// <summary>
/// Represents the result of payment processing in the chain
/// </summary>
public class PaymentProcessingChainResult
{
    public bool IsSuccessful { get; set; }
    public string ErrorMessage { get; set; }
}

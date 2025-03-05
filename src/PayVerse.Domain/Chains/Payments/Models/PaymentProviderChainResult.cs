namespace PayVerse.Domain.Chains.Payments.Models;

public class PaymentProviderChainResult
{
    public bool IsSuccessful { get; set; }
    public string ErrorMessage { get; set; }
}

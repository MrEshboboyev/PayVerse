namespace PayVerse.Infrastructure.Chains.Payments.Gateway;

/// <summary>
/// Payment Gateway Response DTO
/// </summary>
/// <param name="IsSuccessful"></param>
/// <param name="TransactionId"></param>
/// <param name="ErrorMessage"></param>
public sealed record PaymentGatewayResponse(
    bool IsSuccessful, 
    string TransactionId, 
    string ErrorMessage);

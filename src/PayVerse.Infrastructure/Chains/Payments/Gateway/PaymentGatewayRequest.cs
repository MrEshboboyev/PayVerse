namespace PayVerse.Infrastructure.Chains.Payments.Gateway;

/// <summary>
/// Payment Gateway Request DTO
/// </summary>
/// <param name="Amount"></param>
/// <param name="UserId"></param>
/// <param name="PaymentMethod"></param>
public sealed record PaymentGatewayRequest(
    decimal Amount, 
    string UserId, 
    string PaymentMethod);

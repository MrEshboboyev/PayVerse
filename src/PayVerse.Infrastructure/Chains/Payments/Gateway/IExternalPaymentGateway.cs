namespace PayVerse.Infrastructure.Chains.Payments.Gateway;

/// <summary>
/// External Payment Gateway Interface
/// </summary>
public interface IExternalPaymentGateway
{
    Task<PaymentGatewayResponse> ProcessPaymentAsync(PaymentGatewayRequest request);
}

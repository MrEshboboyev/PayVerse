namespace PayVerse.Infrastructure.Chains.Payments.Gateway;

/// <summary>
/// External Payment Gateway Implementation (Mockup)
/// </summary>
public class ExternalPaymentGateway : IExternalPaymentGateway
{
    public async Task<PaymentGatewayResponse> ProcessPaymentAsync(PaymentGatewayRequest request)
    {
        // Simulate payment processing
        await Task.Delay(100); // Simulate network delay

        // Basic validation
        if (request.Amount <= 0)
        {
            return new PaymentGatewayResponse(false, default, "Invalid payment amount");
        }

        // Simulate potential failure scenarios
        if (request.Amount > 10000)
        {
            return new PaymentGatewayResponse(false, default, "Payment amount exceeds limit");
        }

        // Successful payment
        return new PaymentGatewayResponse(true, Guid.NewGuid().ToString(), default);
    }
}
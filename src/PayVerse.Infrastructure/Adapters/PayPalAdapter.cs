using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Infrastructure.Adapters;

public class PayPalAdapter : IPaymentGateway
{
    public Task<bool> ProcessPaymentAsync(Payment payment)
    {
        return true;
    }
}

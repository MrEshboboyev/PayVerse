using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Infrastructure.Decorators;

// ✅ Handles standard payment processing.

public class BasePaymentProcessor : IPaymentProcessor
{
    public void ProcessPayment(Payment payment)
    {
        Console.WriteLine($"Processing payment of ${payment.Amount} for {payment.Id}...");
    }
}

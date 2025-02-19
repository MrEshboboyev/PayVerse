using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Bridges;

public class CryptoProcessor : IPaymentProcessor
{
    public void ProcessPayment(Payment payment)
    {
        Console.WriteLine($"Processing crypto transaction of ${payment.Amount}...");
    }
}

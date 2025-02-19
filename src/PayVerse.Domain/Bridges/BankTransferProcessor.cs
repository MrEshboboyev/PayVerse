using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Bridges;

public class BankTransferProcessor : IPaymentProcessor
{
    public void ProcessPayment(Payment payment)
    {
        Console.WriteLine($"Processing bank transfer of ${payment.Amount}...");
    }
}

using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Infrastructure.Decorators;

// ✅ Enhances payment processing with logging, without modifying the core logic.

public class LoggingPaymentDecorator(IPaymentProcessor processor) : IPaymentProcessor
{
    public void ProcessPayment(Payment payment)
    {
        Console.WriteLine($"[LOG] Payment initiated: {payment.Id} - Amount: ${payment.Amount}");
        processor.ProcessPayment(payment);
        Console.WriteLine($"[LOG] Payment completed: {payment.Id}");
    }
}

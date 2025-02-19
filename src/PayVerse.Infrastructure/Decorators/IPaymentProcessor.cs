using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Infrastructure.Decorators;

// ✅ Ensures all payment processors follow a common structure.

public interface IPaymentProcessor
{
    void ProcessPayment(Payment payment);
}


// ✅ Future Benefits:
// Can extend further(e.g., add security checks, analytics) without modifying existing code.
// Supports layered enhancements dynamically.
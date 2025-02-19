using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Bridges;

// ✅ Benefits:
// Allows adding new payment types(e.g., mobile wallets, cash).
// Supports dynamic payment method selection based on user preference.

public interface IPaymentProcessor
{
    void ProcessPayment(Payment payment);
}

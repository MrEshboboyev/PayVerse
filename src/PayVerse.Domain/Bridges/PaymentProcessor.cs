using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Bridges;

/// <summary>
/// The Abstraction defines the interface for the "control" part of the two class hierarchies.
/// It maintains a reference to an object of the Implementation hierarchy and delegates all the real work to this object.
/// </summary>
public abstract class PaymentProcessor(IPaymentProvider paymentProvider)
{
    protected readonly IPaymentProvider _paymentProvider = paymentProvider;

    public abstract Task<Payment> ProcessPaymentAsync(Payment payment, IDictionary<string, string> paymentDetails);
    public abstract Task<Payment> RefundPaymentAsync(Payment payment, decimal amount);
    public abstract Task<Payment> CheckPaymentStatusAsync(Payment payment);
}

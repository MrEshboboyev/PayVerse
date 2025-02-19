using PayVerse.Domain.Shared;
using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Strategies;

public interface IPaymentStrategy
{
    Task<Result> ProcessPayment(Payment payment);
}
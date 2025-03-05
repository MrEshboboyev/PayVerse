using PayVerse.Domain.Chains.Payments.Models;
using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Chains.Payments.Services;

public interface IPaymentProviderChainService
{
    Task<PaymentProviderChainResult> ProcessPayment(Payment payment);
}

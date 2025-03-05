using PayVerse.Domain.Chains.Payments.Models;
using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Chains.Payments.Services;

public interface IFraudDetectionChainService
{
    Task<FraudCheckChainResult> CheckPaymentForFraud(Payment payment);
}

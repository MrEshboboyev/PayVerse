using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Mementos.Payments;

public interface IPaymentHistoryService
{
    Task<Result> SavePaymentStateAsync(Payment payment, string metadata = "");
    Task<Result<Payment>> RestorePaymentStateAsync(Guid paymentId, int version);
    Task<Result<Payment>> RestoreLatestPaymentStateAsync(Guid paymentId);
    Task<Result<List<PaymentHistoryDto>>> GetPaymentHistoryAsync(Guid paymentId);
}

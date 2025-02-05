using PayVerse.Application.Payments.Queries.Common.Responses;
using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Application.Payments.Queries.Common.Factories;

public static class PaymentResponseFactory
{
    public static PaymentResponse Create(Payment payment)
    {
        return new PaymentResponse(
            payment.Id,
            payment.Amount.Value,
            payment.Status,
            payment.UserId,
            payment.CreatedOnUtc,
            payment.ModifiedOnUtc);
    }
}
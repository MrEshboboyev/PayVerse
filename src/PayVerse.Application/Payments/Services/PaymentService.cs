using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.Payments.Services;

/// <summary>
/// Service for managing payments
/// </summary>
public class PaymentService(IPaymentRepository paymentRepository)
{
    /// <summary>
    /// Processes an immediate payment
    /// </summary>
    public async Task<Guid> ProcessPayment(Guid userId,
                                           decimal amount,
                                           Currency currency,
                                           string provider,
                                           PaymentMethod paymentMethod)
    {
        var payment = Payment.CreateBuilder(userId, amount)
            .WithStatus(PaymentStatus.Processing)
            .WithProvider(provider)
            .WithPaymentMethod(paymentMethod)
            .Build();

        await paymentRepository.AddAsync(payment);

        // Process payment logic...

        return payment.Id;
    }

    /// <summary>
    /// Schedules a future payment
    /// </summary>
    public async Task<Guid> SchedulePayment(Guid userId,
                                            decimal amount,
                                            Currency currency,
                                            DateTime scheduledDate,
                                            PaymentMethod paymentMethod)
    {
        var payment = Payment.CreateBuilder(userId, amount)
            .ScheduleFor(scheduledDate)
            .WithPaymentMethod(paymentMethod)
            .Build();

        await paymentRepository.AddAsync(payment);

        return payment.Id;
    }
}
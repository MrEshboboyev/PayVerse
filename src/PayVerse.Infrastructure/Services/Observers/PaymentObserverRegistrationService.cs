using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Observers;

namespace PayVerse.Infrastructure.Services.Observers;

/// <summary>
/// Service that registers observers with payment subjects
/// </summary>
public class PaymentObserverRegistrationService(IEnumerable<IObserver> observers) : IPaymentObserverRegistrationService
{
    public async Task RegisterObserversAsync(Payment payment)
    {
        foreach (var observer in observers)
        {
            await payment.AttachAsync(observer);
        }
    }
}
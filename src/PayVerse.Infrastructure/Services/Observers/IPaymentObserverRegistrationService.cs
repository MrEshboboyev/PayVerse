using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Infrastructure.Services.Observers;


/// <summary>
/// Interface for a service that registers observers with payment subjects
/// </summary>
public interface IPaymentObserverRegistrationService
{
    Task RegisterObserversAsync(Payment payment);
}

using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Domain.Observers.VirtualAccounts;


/// <summary>
/// Observer that updates virtual account balance when payment status changes
/// </summary>
public class VirtualAccountBalanceObserver(
    IVirtualAccountRepository virtualAccountRepository) : IObserver
{
    public async Task UpdateAsync(ISubject subject)
    {
        if (subject is Payment payment)
        {
            // Only react to processed or refunded payments
            if (payment.Status == PaymentStatus.Processed || payment.Status == PaymentStatus.Refunded)
            {
                // Find the user's virtual account
                var virtualAccounts = await virtualAccountRepository.GetAllByUserIdAsync(payment.UserId);

                var virtualAccount = virtualAccounts.First();

                if (virtualAccount != null)
                {
                    if (payment.Status == PaymentStatus.Processed)
                    {
                        // Add the payment amount to the virtual account
                        virtualAccount.AddTransaction(
                            payment.Amount,
                            DateTime.UtcNow,
                            $"Payment processed: {payment.TransactionId}");
                    }
                    else if (payment.Status == PaymentStatus.Refunded)
                    {
                        // Subtract the payment amount from the virtual account
                        virtualAccount.AddTransaction(
                            Amount.Create(-payment.Amount.Value).Value,
                            DateTime.UtcNow,
                            $"Payment refunded: {payment.RefundTransactionId}");
                    }

                    await virtualAccountRepository.UpdateAsync(virtualAccount);
                }
            }
        }
    }
}
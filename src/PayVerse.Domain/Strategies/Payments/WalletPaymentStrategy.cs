using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Strategies.ValueObjects;

namespace PayVerse.Domain.Strategies.Payments;

/// <summary>
/// Strategy for processing wallet payments within the system
/// </summary>
public class WalletPaymentStrategy(
    IWalletRepository walletRepository,
    IUnitOfWork unitOfWork)
    : IPaymentProcessingStrategy
{
    public async Task<PaymentResult> ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        // Wallet payment specific processing logic
        var walletDetails = payment.PaymentMethodEntity.GetWalletDetails();
        var wallet = await walletRepository.GetByIdAsync(walletDetails.WalletId, cancellationToken);

        if (wallet == null)
        {
            return new PaymentResult(false, null, "Wallet not found");
        }

        var result = wallet.DeductFunds(payment.Amount, string.Empty);

        if (!result.IsSuccess)
        {
            return new PaymentResult(false, null, result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new PaymentResult(
            true,
            Guid.NewGuid().ToString(), // Internal transaction ID
            null);
    }
}
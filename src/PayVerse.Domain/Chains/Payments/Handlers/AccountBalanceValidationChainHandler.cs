using PayVerse.Domain.Chains.Payments.Models;
using PayVerse.Domain.Chains.Payments.Services;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Domain.Chains.Payments.Handlers;

/// <summary>
/// Checks user account balance and spending limits in the processing chain
/// </summary>
public class AccountBalanceValidationChainHandler(
    IVirtualAccountChainService virtualAccountChainService) : PaymentProcessingChainHandler
{
    public override async Task<PaymentProcessingChainResult> ProcessPaymentInChain(Payment payment)
    {
        // Retrieve user's virtual account
        var virtualAccount = await virtualAccountChainService.GetVirtualAccountByUserId(payment.UserId);

        // Check if account has sufficient balance
        if (virtualAccount.Balance.Value < payment.Amount.Value)
        {
            // Check if overdraft is available
            if (!HasSufficientOverdraftLimit(virtualAccount, payment))
            {
                return new PaymentProcessingChainResult
                {
                    IsSuccessful = false,
                    ErrorMessage = "Insufficient funds"
                };
            }
        }

        // If no issues, pass to next handler
        return _nextChainHandler != null
            ? await _nextChainHandler.ProcessPaymentInChain(payment)
            : new PaymentProcessingChainResult { IsSuccessful = true };
    }

    private bool HasSufficientOverdraftLimit(VirtualAccount account, Payment payment)
    {
        return account.OverdraftLimit >= payment.Amount.Value - account.Balance.Value;
    }
}

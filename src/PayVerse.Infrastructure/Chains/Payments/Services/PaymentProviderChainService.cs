using PayVerse.Domain.Chains.Payments.Models;
using PayVerse.Domain.Chains.Payments.Services;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Infrastructure.Chains.Payments.Gateway;
using PayVerse.Persistence;

namespace PayVerse.Infrastructure.Chains.Payments.Services;

/// <summary>
/// Payment Provider Chain Service implementations
/// </summary>
public class PaymentProviderChainService(
    ApplicationDbContext context,
    IExternalPaymentGateway externalPaymentGateway) : IPaymentProviderChainService
{
    public async Task<PaymentProviderChainResult> ProcessPayment(Payment payment)
    {
        try
        {
            // Call external payment gateway
            var gatewayResult = await externalPaymentGateway.ProcessPaymentAsync(
                new PaymentGatewayRequest(
                    payment.Amount.Value, 
                    payment.UserId.ToString(), 
                    payment.PaymentMethod.ToString()!));

            // Update payment status
            if (gatewayResult.IsSuccessful)
            {
                payment.UpdateStatus(PaymentStatus.Completed);
            }
            else
            {
                payment.UpdateStatus(PaymentStatus.Failed);
            }

            // Save transaction details
            payment.MarkAsProcessed(gatewayResult.TransactionId);

            // Save changes to database
            context.Set<Payment>().Update(payment);
            await context.SaveChangesAsync();

            return new PaymentProviderChainResult
            {
                IsSuccessful = gatewayResult.IsSuccessful,
                ErrorMessage = gatewayResult.ErrorMessage
            };
        }
        catch (Exception ex)
        {
            // Log the exception
            // In a real-world scenario, use a proper logging mechanism
            return new PaymentProviderChainResult
            {
                IsSuccessful = false,
                ErrorMessage = $"Payment processing error: {ex.Message}"
            };
        }
    }
}

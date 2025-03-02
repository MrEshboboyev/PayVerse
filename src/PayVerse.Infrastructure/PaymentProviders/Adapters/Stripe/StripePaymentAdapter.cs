using PayVerse.Domain.Adapters.Payments;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Infrastructure.PaymentProviders.Adapters.Stripe;

// The Adapter class - adapts Stripe's interface to our PayVerse interface
public class StripePaymentAdapter(StripePaymentService stripeService) : IPaymentGatewayAdapter
{
    public async Task<PaymentProcessResult> ProcessPaymentAsync(Payment payment)
    {
        try
        {
            // Convert from our domain model to what Stripe expects
            var stripeResult = await stripeService.CreateChargeAsync(
                payment.UserId.ToString(),
                payment.Amount.Value,
                "USD", // Hardcoded currency for simplicity
                $"Payment {payment.Id}");

            // Convert from Stripe's response to our domain model
            return new PaymentProcessResult
            {
                IsSuccess = stripeResult.Success,
                TransactionId = stripeResult.ChargeId,
                ErrorMessage = stripeResult.FailureMessage,
                ProcessedDate = stripeResult.Created
            };
        }
        catch (Exception ex)
        {
            return new PaymentProcessResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message,
                ProcessedDate = DateTime.UtcNow
            };
        }
    }

    public async Task<RefundProcessResult> ProcessRefundAsync(Payment payment)
    {
        try
        {
            if (string.IsNullOrEmpty(payment.TransactionId))
            {
                return new RefundProcessResult
                {
                    IsSuccess = false,
                    ErrorMessage = "No transaction ID found for refund",
                    RefundedDate = DateTime.UtcNow
                };
            }

            var stripeResult = await stripeService.CreateRefundAsync(
                payment.TransactionId,
                payment.Amount.Value);

            return new RefundProcessResult
            {
                IsSuccess = stripeResult.Success,
                RefundTransactionId = stripeResult.RefundId,
                ErrorMessage = stripeResult.FailureMessage,
                RefundedDate = stripeResult.Created
            };
        }
        catch (Exception ex)
        {
            return new RefundProcessResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message,
                RefundedDate = DateTime.UtcNow
            };
        }
    }

    public async Task<PaymentVerificationResult> VerifyPaymentAsync(string transactionId)
    {
        try
        {
            var stripeStatus = await stripeService.RetrieveChargeAsync(transactionId);

            PaymentStatus status = stripeStatus.Status switch
            {
                "succeeded" => PaymentStatus.Processed,
                "pending" => PaymentStatus.Pending,
                "failed" => PaymentStatus.Failed,
                _ => PaymentStatus.Unknown
            };

            return new PaymentVerificationResult
            {
                IsVerified = status == PaymentStatus.Processed,
                Status = status,
                ErrorMessage = status == PaymentStatus.Failed 
                    ? "Payment failed at provider" 
                    : string.Empty
            };
        }
        catch (Exception ex)
        {
            return new PaymentVerificationResult
            {
                IsVerified = false,
                Status = PaymentStatus.Unknown,
                ErrorMessage = ex.Message
            };
        }
    }
}

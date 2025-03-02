using PayVerse.Domain.Adapters.Payments;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Infrastructure.PaymentProviders.Adapters.PayPal;

// PayPal Adapter - another example of adapting a different payment system
public class PayPalPaymentAdapter(PayPalService payPalService) : IPaymentGatewayAdapter
{
    public async Task<PaymentProcessResult> ProcessPaymentAsync(Payment payment)
    {
        try
        {
            var request = new PayPalPaymentRequest
            {
                CustomerId = payment.UserId.ToString(),
                Amount = payment.Amount.Value,
                Currency = "USD", // Assume USD for now
                Description = $"Payment {payment.Id}"
            };

            var payPalResult = await payPalService.MakePaymentAsync(request);

            return new PaymentProcessResult
            {
                IsSuccess = payPalResult.Status == "COMPLETED",
                TransactionId = payPalResult.PaymentId,
                ErrorMessage = payPalResult.ErrorMessage,
                ProcessedDate = payPalResult.Timestamp
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

            var payPalResult = await payPalService.RefundPaymentAsync(
                payment.TransactionId,
                payment.Amount.Value);

            return new RefundProcessResult
            {
                IsSuccess = payPalResult.Status == "COMPLETED",
                RefundTransactionId = payPalResult.RefundId,
                ErrorMessage = payPalResult.ErrorMessage,
                RefundedDate = payPalResult.Timestamp
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
            var payPalStatus = await payPalService.GetPaymentStatusAsync(transactionId);

            PaymentStatus status = payPalStatus.Status switch
            {
                "COMPLETED" => PaymentStatus.Processed,
                "PENDING" => PaymentStatus.Pending,
                "FAILED" => PaymentStatus.Failed,
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
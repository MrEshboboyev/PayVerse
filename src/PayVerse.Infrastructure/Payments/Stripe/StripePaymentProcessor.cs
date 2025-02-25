using Microsoft.Extensions.Options;
using PayVerse.Domain.Abstractions.Payments;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Shared;
using PayVerse.Infrastructure.Payments.Stripe.Models;
using Stripe;

namespace PayVerse.Infrastructure.Payments.Stripe;

/// <summary>
/// Implements the payment processor for Stripe.
/// </summary>
internal sealed class StripePaymentProcessor : IPaymentProcessor
{
    private readonly StripeSettings _settings;
     
    public StripePaymentProcessor(IOptions<StripeSettings> settings)
    {
        _settings = settings.Value;
        StripeConfiguration.ApiKey = _settings.SecretKey;
    }

    public async Task<Result> ProcessPaymentAsync(Payment payment)
    {
        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(payment.Amount.Value * 100), // Stripe uses cents
                Currency = "usd",
                PaymentMethod = "pm_card_visa", // This would come from the client in a real app
                Confirm = true,
                Description = $"Payment {payment.Id}",
                Metadata = new Dictionary<string, string>
                {
                    { "PaymentId", payment.Id.ToString() },
                    { "UserId", payment.UserId.ToString() }
                }
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            if (paymentIntent.Status == "succeeded")
            {
                return Result.Success();
            }

            return Result.Failure(
                DomainErrors.Stripe.PaymentFailedWithStatus(paymentIntent.Status));
        }
        catch (StripeException ex)
        {
            return Result.Failure(
                DomainErrors.Stripe.PaymentProcessingError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure(
                DomainErrors.Stripe.UnexpectedPaymentProcessingError(ex.Message));
        }
    }

    public async Task<Result> ValidatePaymentAsync(Payment payment)
    {
        // Validate payment amount, currency, etc.
        if (payment.Amount.Value <= 0)
        {
            return Result.Failure(DomainErrors.Stripe.PaymentAmountMustBeGreaterThanZero);
        }

        // In real implementation, we might validate the payment method or customer details

        return Result.Success();
    }

    public async Task<Result> CancelPaymentAsync(Payment payment)
    {
        try
        {
            // In a real implementation, we would use the stored Stripe payment intent ID
            // For demonstration, we'll assume we have a paymentIntentId from somewhere
            string paymentIntentId = "pi_MOCK_" + payment.Id;

            var service = new PaymentIntentService();
            var paymentIntent = await service.CancelAsync(paymentIntentId);

            if (paymentIntent.Status == "canceled")
            {
                return Result.Success();
            }

            return Result.Failure(
                DomainErrors.Stripe.PaymentCancellationFailed(paymentIntent.Status));
        }
        catch (StripeException ex)
        {
            return Result.Failure(
                DomainErrors.Stripe.PaymentCancellationError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure(
                DomainErrors.Stripe.UnexpectedPaymentCancellationError(ex.Message));
        }
    }

    public async Task<Result> RefundPaymentAsync(Payment payment)
    {
        try
        {
            // In a real implementation, we would use the stored Stripe payment intent ID
            // For demonstration, we'll assume we have a paymentIntentId from somewhere
            string paymentIntentId = "pi_MOCK_" + payment.Id;

            var options = new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId,
                Amount = (long)(payment.Amount.Value * 100) // Stripe uses cents
            };

            var service = new RefundService();
            var refund = await service.CreateAsync(options);

            if (refund.Status == "succeeded")
            {
                return Result.Success();
            }

            return Result.Failure(
                DomainErrors.Stripe.RefundFailedWithStatus(refund.Status));
        }
        catch (StripeException ex)
        {
            return Result.Failure(
                DomainErrors.Stripe.RefundError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure(
                DomainErrors.Stripe.UnexpectedRefundError(ex.Message));
        }
    }
}
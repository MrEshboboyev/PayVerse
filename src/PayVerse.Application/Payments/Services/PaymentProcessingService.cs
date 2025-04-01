using PayVerse.Domain.Abstractions.Payments;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Payments.Services;

/// <summary>
/// Service for processing payments using the Abstract Factory pattern.
/// </summary>
public sealed class PaymentProcessingService(
    IPaymentProviderManager paymentProviderManager,
    IPaymentRepository paymentRepository)
{

    /// <summary>
    /// Processes a payment through the specified payment provider.
    /// </summary>
    /// <param name="payment">The payment to process.</param>
    /// <param name="providerName">Optional provider name. If not specified, the default provider is used.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the transaction ID.</returns>
    public async Task<Result<string>> ProcessPaymentAsync(
        Payment payment,
        string providerName = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Domain validation
            var canBeProcessedResult = payment.CanBeProcessed();
            if (!canBeProcessedResult.IsSuccess)
            {
                return Result.Failure<string>(canBeProcessedResult.Error);
            }

            // Get the appropriate factory (default or specified)
            var factory = string.IsNullOrEmpty(providerName)
                ? paymentProviderManager.GetDefaultFactory()
                : paymentProviderManager.GetFactory(providerName);

            // Create payment processor from factory
            var paymentProcessor = factory.CreatePaymentProcessor();

            // Validate the payment via payment processor
            var validationResult = await paymentProcessor.ValidatePaymentAsync(payment);
            if (!validationResult.IsSuccess)
            {
                payment.RecordFailure(validationResult.Error);
                await paymentRepository.UpdateAsync(payment, cancellationToken);
                return Result.Failure<string>(validationResult.Error);
            }

            // Process the payment
            var paymentResult = await paymentProcessor.ProcessPaymentAsync(payment);
            if (!paymentResult.IsSuccess)
            {
                payment.RecordFailure(paymentResult.Error);
                await paymentRepository.UpdateAsync(payment, cancellationToken);
                return Result.Failure<string>(paymentResult.Error);
            }

            // Create transaction record
            var transactionManager = factory.CreateTransactionManager();
            var transactionResult = await transactionManager.CreateTransactionAsync(payment);

            if (!transactionResult.IsSuccess)
            {
                var error = DomainErrors.Payment.TransactionCreationFailed(transactionResult.Error);
                payment.RecordFailure(error);
                await paymentRepository.UpdateAsync(payment, cancellationToken);
                return Result.Failure<string>(error);
            }

            // Update payment with transaction details and mark as completed
            payment.RecordTransactionDetails(transactionResult.Value, factory.ProviderName);
            payment.UpdateStatus(PaymentStatus.Completed);

            // Save updated payment to repository
            await paymentRepository.UpdateAsync(payment, cancellationToken);

            return Result<string>.Success(transactionResult.Value);
        }
        catch (KeyNotFoundException ex)
        {
            var error = DomainErrors.Payment.ProviderNotFound(ex.Message);
            payment.RecordFailure(error);
            await paymentRepository.UpdateAsync(payment, cancellationToken);
            return Result.Failure<string>(error);
        }
        catch (InvalidOperationException ex)
        {
            var error = DomainErrors.Payment.ConfigurationError(ex.Message);
            payment.RecordFailure(error);
            await paymentRepository.UpdateAsync(payment, cancellationToken);
            return Result.Failure<string>(error);
        }
        catch (Exception ex)
        {
            var error = DomainErrors.Payment.UnexpectedError(ex.Message);
            payment.RecordFailure(error);
            await paymentRepository.UpdateAsync(payment, cancellationToken);
            return Result.Failure<string>(error);
        }
    }

    /// <summary>
    /// Refunds a payment through the provider that processed the original payment.
    /// </summary>
    /// <param name="paymentId">The ID of the payment to refund.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    public async Task<Result> RefundPaymentAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get the original payment record
            var payment = await paymentRepository.GetByIdAsync(paymentId, cancellationToken);

            if (payment == null)
            {
                return Result.Failure(DomainErrors.Payment.NotFound(paymentId));
            }

            // Domain validation
            var canBeRefundedResult = payment.CanBeRefunded();
            if (!canBeRefundedResult.IsSuccess)
            {
                return Result.Failure(canBeRefundedResult.Error);
            }

            // Get the factory for the provider that processed the original payment
            var factory = paymentProviderManager.GetFactory(payment.ProviderName);
            var paymentProcessor = factory.CreatePaymentProcessor();

            // Process the refund
            var refundResult = await paymentProcessor.RefundPaymentAsync(payment);

            if (!refundResult.IsSuccess)
            {
                return Result.Failure(refundResult.Error);
            }

            // Create a transaction record for the refund
            var transactionManager = factory.CreateTransactionManager();
            var transactionResult = await transactionManager.CreateTransactionAsync(payment);

            if (!transactionResult.IsSuccess)
            {
                return Result.Failure(DomainErrors.Payment.RefundTransactionCreationFailed(transactionResult.Error));
            }

            // Update payment with refund details
            payment.RecordRefundDetails(transactionResult.Value);
            payment.UpdateStatus(PaymentStatus.Refunded);

            // Save updated payment to repository
            await paymentRepository.UpdateAsync(payment, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(DomainErrors.Payment.RefundProcessingFailed(ex.Message));
        }
    }

    /// <summary>
    /// Cancels a payment through the specified provider.
    /// </summary>
    /// <param name="paymentId">The ID of the payment to cancel.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    public async Task<Result> CancelPaymentAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get the payment record
            var payment = await paymentRepository.GetByIdAsync(paymentId, cancellationToken);

            if (payment == null)
            {
                return Result.Failure(DomainErrors.Payment.NotFound(paymentId));
            }

            // Domain validation
            var canBeCancelledResult = payment.CanBeCancelled();
            if (!canBeCancelledResult.IsSuccess)
            {
                return Result.Failure(canBeCancelledResult.Error);
            }

            // If no provider yet, just mark as cancelled
            if (string.IsNullOrEmpty(payment.ProviderName))
            {
                payment.UpdateStatus(PaymentStatus.Cancelled);
                await paymentRepository.UpdateAsync(payment, cancellationToken);
                return Result.Success();
            }

            // Get the factory for the provider that processed the payment
            var factory = paymentProviderManager.GetFactory(payment.ProviderName);
            var paymentProcessor = factory.CreatePaymentProcessor();

            // Cancel the payment
            var cancelResult = await paymentProcessor.CancelPaymentAsync(payment);

            if (!cancelResult.IsSuccess)
            {
                return Result.Failure(DomainErrors.Payment.PaymentCancellationFailed(cancelResult.Error));
            }

            // Update payment status
            payment.UpdateStatus(PaymentStatus.Cancelled);

            // Save updated payment to repository
            await paymentRepository.UpdateAsync(payment, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(DomainErrors.Payment.PaymentCancellationError(ex.Message));
        }
    }

    /// <summary>
    /// Generates a receipt for a completed payment.
    /// </summary>
    /// <param name="paymentId">The ID of the payment.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the receipt as a string.</returns>
    public async Task<Result<string>> GenerateReceiptAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get the payment record
            var payment = await paymentRepository.GetByIdAsync(paymentId, cancellationToken);

            if (payment == null)
            {
                return Result.Failure<string>(DomainErrors.Payment.NotFound(paymentId));
            }

            if (string.IsNullOrEmpty(payment.TransactionId))
            {
                return Result.Failure<string>(DomainErrors.Payment.TransactionIdMissing);
            }

            if (string.IsNullOrEmpty(payment.ProviderName))
            {
                return Result.Failure<string>(DomainErrors.Payment.ProviderNameMissing);
            }

            if (payment.Status != PaymentStatus.Completed && payment.Status != PaymentStatus.Refunded)
            {
                return Result.Failure<string>(DomainErrors.Payment.InvalidPaymentStatusForReceipt(payment.Status));
            }

            // Get the factory for the provider that processed the payment
            var factory = paymentProviderManager.GetFactory(payment.ProviderName);
            var transactionManager = factory.CreateTransactionManager();

            // Generate receipt
            return await transactionManager.GenerateReceiptAsync(payment.TransactionId);
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(DomainErrors.Payment.ReceiptGenerationError(ex.Message));
        }
    }

    /// <summary>
    /// Gets the status of a payment transaction from the provider.
    /// </summary>
    /// <param name="paymentId">The ID of the payment.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the transaction status.</returns>
    public async Task<Result<string>> GetTransactionStatusAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get the payment record
            var payment = await paymentRepository.GetByIdAsync(paymentId, cancellationToken);

            if (payment == null)
            {
                return Result.Failure<string>(DomainErrors.Payment.NotFound(paymentId));
            }

            if (string.IsNullOrEmpty(payment.TransactionId))
            {
                return Result.Failure<string>(DomainErrors.Payment.TransactionIdMissing);
            }

            if (string.IsNullOrEmpty(payment.ProviderName))
            {
                return Result.Failure<string>(DomainErrors.Payment.ProviderNameMissing);
            }

            // Get the factory for the provider that processed the payment
            var factory = paymentProviderManager.GetFactory(payment.ProviderName);
            var transactionManager = factory.CreateTransactionManager();

            // Get transaction status from provider
            var providerStatusResult = await transactionManager.GetTransactionStatusAsync(payment.TransactionId);

            if (providerStatusResult.IsSuccess)
            {
                // Update local status if needed (implementation would depend on how provider status maps to domain status)
                // This is a simplified example assuming provider returns a status that matches our enum
                if (Enum.TryParse<PaymentStatus>(providerStatusResult.Value, true, out var providerStatus) &&
                    payment.Status != providerStatus)
                {
                    payment.UpdateStatus(providerStatus);
                    await paymentRepository.UpdateAsync(payment, cancellationToken);
                }
            }

            return providerStatusResult;
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(DomainErrors.Payment.TransactionStatusRetrievalError(ex.Message));
        }
    }
}
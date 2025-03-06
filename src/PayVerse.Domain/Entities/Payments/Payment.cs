using PayVerse.Domain.Builders.Payments;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Events.Payments;
using PayVerse.Domain.Mementos;
using PayVerse.Domain.Mementos.Payments;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Prototypes;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;
using System.Text.Json;

namespace PayVerse.Domain.Entities.Payments;

/// <summary>
/// Represents a payment in the system with Prototype pattern implementation
/// </summary>
public sealed class Payment : PrototypeAggregateRoot, IAuditableEntity, IOriginator<PaymentMemento>
{
    #region Constructors

    private Payment(
        Guid id,
        Amount amount,
        PaymentStatus status,
        Guid userId,
        DateTime? scheduledDate = null)
        : base(id)
    {
        Amount = amount;
        Status = status;
        UserId = userId;
        ScheduledDate = scheduledDate;
        RaiseDomainEvent(new PaymentInitiatedDomainEvent(
            Guid.NewGuid(),
            id));
    }

    // Copy constructor for Prototype pattern
    private Payment(Payment source) : base(source.Id)
    {
        Amount = source.Amount;
        Status = source.Status;
        UserId = source.UserId;
        ScheduledDate = source.ScheduledDate;
        TransactionId = source.TransactionId;
        RefundTransactionId = source.RefundTransactionId;
        ProviderName = source.ProviderName;
        ProcessedDate = source.ProcessedDate;
        RefundedDate = source.RefundedDate;
        CancelledDate = source.CancelledDate;
        FailureReason = source.FailureReason;
        PaymentMethod = source.PaymentMethod;
        CreatedOnUtc = source.CreatedOnUtc;
        ModifiedOnUtc = source.ModifiedOnUtc;
    }

    #endregion

    #region Properties

    public Amount Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public Guid UserId { get; private set; }
    public Guid? InvoiceId { get; private set; }
    public DateTime? ScheduledDate { get; private set; }
    public string TransactionId { get; private set; }
    public string RefundTransactionId { get; private set; }
    public string ProviderName { get; private set; }
    public DateTime? ProcessedDate { get; private set; }
    public DateTime? RefundedDate { get; private set; }
    public DateTime? CancelledDate { get; private set; }
    public string FailureReason { get; private set; }
    public PaymentMethod? PaymentMethod { get; private set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    #endregion

    #region Factory Method

    public static Payment Create(
        Guid id,
        Amount amount,
        PaymentStatus status,
        Guid userId,
        DateTime? scheduledDate = null)
    {
        return new Payment(id, amount, status, userId, scheduledDate);
    }

    #region Prototypes

    // Factory method to create from a prototype
    public static Payment CreateFromPrototype(Payment prototype)
    {
        return prototype.DeepCopy() as Payment;
    }

    // Factory method to create a recurring payment from a prototype
    public static Payment CreateRecurringFromPrototype(Payment prototype,
                                                       DateTime newScheduledDate)
    {
        var newPayment = prototype.DeepCopy() as Payment;
        newPayment.SetScheduledDate(newScheduledDate);
        newPayment.SetStatus(PaymentStatus.Scheduled);
        return newPayment;
    }

    #endregion

    #endregion

    #region Own Methods

    #region Prototype related

    public Result SetStatus(PaymentStatus status)
    {
        Status = status;

        switch (status)
        {
            case PaymentStatus.Processed:
                ProcessedDate = DateTime.UtcNow;
                break;
            case PaymentStatus.Refunded:
                RefundedDate = DateTime.UtcNow;
                break;
            case PaymentStatus.Cancelled:
                CancelledDate = DateTime.UtcNow;
                break;
        }

        return Result.Success();
    }

    public Result SetScheduledDate(DateTime scheduledDate)
    {
        ScheduledDate = scheduledDate;

        return Result.Success();
    }

    #endregion

    /// <summary>
    /// Updates the payment status with appropriate domain events.
    /// </summary>
    /// <param name="newStatus">The new payment status.</param>
    /// <returns>Result indicating success or failure.</returns>
    public Result UpdateStatus(PaymentStatus newStatus)
    {
        // Can't change status if already in a terminal state
        if (IsInTerminalState() && newStatus != Status)
        {
            return Result.Failure(
                DomainErrors.Payment.CannotChangeStatusFromTerminalState(Status, newStatus));
        }

        if (Status == newStatus)
            return Result.Success();

        var oldStatus = Status;
        Status = newStatus;

        // Set appropriate timestamps based on status
        switch (newStatus)
        {
            case PaymentStatus.Processed:
                ProcessedDate = DateTime.UtcNow;
                RaiseDomainEvent(new PaymentProcessedDomainEvent(
                    Guid.NewGuid(),
                    Id));
                break;
            case PaymentStatus.Refunded:
                RefundedDate = DateTime.UtcNow;
                RaiseDomainEvent(new PaymentRefundedDomainEvent(
                    Guid.NewGuid(),
                    Id,
                    "NONE"));
                break;
            case PaymentStatus.Cancelled:
                CancelledDate = DateTime.UtcNow;
                RaiseDomainEvent(new PaymentCancelledDomainEvent(
                    Guid.NewGuid(),
                    Id));
                break;
            case PaymentStatus.Failed:
                RaiseDomainEvent(new PaymentFailedDomainEvent(
                    Guid.NewGuid(),
                    Id,
                    FailureReason ?? "Unknown failure"));
                break;
        }

        RaiseDomainEvent(new PaymentStatusUpdatedDomainEvent(
            Guid.NewGuid(),
            Id,
            oldStatus,
            newStatus));

        return Result.Success();
    }

    /// <summary>
    /// Processes a refund for the payment.
    /// </summary>
    /// <param name="refundReason">The reason for the refund.</param>
    /// <returns>Result indicating success or failure.</returns>
    public Result Refund(
        string refundTransactionId,
        string refundReason)
    {
        // Ensure payment is in a refundable state
        if (Status != PaymentStatus.Processed)
        {
            return Result.Failure(
                DomainErrors.Payment.CannotRefundFromCurrentStatus(Status));
        }

        // Update status and timestamps
        var oldStatus = Status;
        Status = PaymentStatus.Refunded;
        RefundTransactionId = refundTransactionId;
        RefundedDate = DateTime.UtcNow;

        // Raise refund domain event
        RaiseDomainEvent(new PaymentRefundedDomainEvent(
            Guid.NewGuid(),
            Id,
            refundReason));

        // Raise status updated event
        RaiseDomainEvent(new PaymentStatusUpdatedDomainEvent(
            Guid.NewGuid(),
            Id,
            oldStatus,
            PaymentStatus.Refunded));

        return Result.Success();
    }


    /// <summary>
    /// Records transaction details from a payment provider.
    /// </summary>
    /// <param name="transactionId">The transaction ID from the provider.</param>
    /// <param name="providerName">The name of the payment provider.</param>
    /// <returns>Result indicating success or failure.</returns>
    public Result RecordTransactionDetails(string transactionId, string providerName)
    {
        if (string.IsNullOrEmpty(transactionId))
        {
            return Result.Failure(
                DomainErrors.Payment.TransactionIdCannotBeEmpty);
        }

        if (string.IsNullOrEmpty(providerName))
        {
            return Result.Failure(
                DomainErrors.Payment.ProviderNameCannotBeEmpty);
        }

        TransactionId = transactionId;
        ProviderName = providerName;

        return Result.Success();
    }

    /// <summary>
    /// Records refund transaction details.
    /// </summary>
    /// <param name="refundTransactionId">The refund transaction ID.</param>
    /// <returns>Result indicating success or failure.</returns>
    public Result RecordRefundDetails(string refundTransactionId)
    {
        if (string.IsNullOrEmpty(refundTransactionId))
        {
            return Result.Failure(
                DomainErrors.Payment.RefundTransactionIdCannotBeEmpty);
        }

        RefundTransactionId = refundTransactionId;

        return Result.Success();
    }

    /// <summary>
    /// Records a payment failure with reason.
    /// </summary>
    /// <param name="failureReason">The reason for the payment failure.</param>
    /// <returns>Result indicating the status update.</returns>
    public Result RecordFailure(string failureReason)
    {
        FailureReason = failureReason;
        return UpdateStatus(PaymentStatus.Failed);
    }

    /// <summary>
    /// Sets the payment method for this payment.
    /// </summary>
    /// <param name="paymentMethod">The payment method.</param>
    /// <returns>Result indicating success or failure.</returns>
    public Result SetPaymentMethod(PaymentMethod paymentMethod)
    {
        PaymentMethod = paymentMethod;
        return Result.Success();
    }

    /// <summary>
    /// Sets the provider name for this payment.
    /// </summary>
    /// <param name="providerName">The Provider name.</param>
    /// <returns>Result indicating success or failure.</returns>
    public Result SetProviderName(string providerName)
    {
        ProviderName = providerName;

        return Result.Success();
    }

    // Methods to support the bridge pattern
    public Result SetTransactionId(string transactionId)
    {
        TransactionId = transactionId;

        return Result.Success();
    }

    /// <summary>
    /// Determines if the payment is in a terminal state (can't be changed).
    /// </summary>
    /// <returns>True if in terminal state, false otherwise.</returns>
    private bool IsInTerminalState()
    {
        return Status == PaymentStatus.Completed ||
               Status == PaymentStatus.Refunded ||
               Status == PaymentStatus.Failed ||
               Status == PaymentStatus.Cancelled;
    }

    /// <summary>
    /// Validates if the payment can be processed.
    /// </summary>
    /// <returns>Result indicating if payment can be processed.</returns>
    public Result CanBeProcessed()
    {
        if (IsInTerminalState())
        {
            return Result.Failure(
                DomainErrors.Payment.PaymentInTerminalState(Status));
        }

        if (Amount.Value <= 0)
        {
            return Result.Failure(DomainErrors.Payment.AmountMustBeGreaterThanZero);
        }

        return Result.Success();
    }

    /// <summary>
    /// Validates if the payment can be refunded.
    /// </summary>
    /// <returns>Result indicating if payment can be refunded.</returns>
    public Result CanBeRefunded()
    {
        if (Status != PaymentStatus.Completed)
        {
            return Result.Failure(DomainErrors.Payment.OnlyCompletedPaymentsCanBeRefunded(Status));
        }

        if (string.IsNullOrEmpty(TransactionId))
        {
            return Result.Failure(DomainErrors.Payment.CannotRefundWithoutTransactionId);
        }

        return Result.Success();
    }

    /// <summary>
    /// Validates if the payment can be cancelled.
    /// </summary>
    /// <returns>Result indicating if payment can be cancelled.</returns>
    public Result CanBeCancelled()
    {
        if (IsInTerminalState())
        {
            return Result.Failure(DomainErrors.Payment.CannotBeCancelledInTerminalState(Status));
        }

        return Result.Success();
    }

    #region Status related

    public Result MarkAsProcessing()
    {
        Status = PaymentStatus.Processing;
        ProcessedDate = DateTime.UtcNow;

        RaiseDomainEvent(new PaymentProcessingDomainEvent(
            Guid.NewGuid(),
            Id));

        return Result.Success();
    }

    public Result MarkAsProcessed(string transactionId)
    {
        Status = PaymentStatus.Processed;
        TransactionId = transactionId;
        ProcessedDate = DateTime.UtcNow;

        RaiseDomainEvent(new PaymentProcessedDomainEvent(
            Guid.NewGuid(),
            Id));

        return Result.Success();
    }

    public Result MarkAsFailed(string reason)
    {
        Status = PaymentStatus.Failed;
        FailureReason = reason;

        RaiseDomainEvent(new PaymentFailedDomainEvent(
            Guid.NewGuid(),
            Id,
            reason));

        return Result.Success();
    }

    public Result MarkAsRefunded()
    {
        Status = PaymentStatus.Refunded;
        RefundedDate = DateTime.UtcNow;

        RaiseDomainEvent(new PaymentRefundedDomainEvent(
            Guid.NewGuid(),
            Id,
            "NONE"));

        return Result.Success();
    }

    #endregion

    #endregion

    #region Invoice related

    /// <summary>
    /// Links the payment to an invoice.
    /// </summary>
    /// <param name="invoiceId">The ID of the invoice to link.</param>
    public Result LinkToInvoice(Guid invoiceId)
    {
        InvoiceId = invoiceId;

        RaiseDomainEvent(new PaymentLinkedToInvoiceDomainEvent(
            Guid.NewGuid(), 
            Id, 
            invoiceId));

        return Result.Success();
    }


    #endregion

    #region Prototype overrides

    public override PrototypeAggregateRoot ShallowCopy()
    {
        return new Payment(
            Id,
            Amount,
            Status,
            UserId,
            ScheduledDate);
    }

    public override PrototypeAggregateRoot DeepCopy()
    {
        return new Payment(this);
    }

    #endregion

    #region Builders

    // Factory method for the builder
    public static PaymentBuilder CreateBuilder(Guid userId,
                                               decimal amount)
    {
        return new PaymentBuilder(userId, amount); // add currency - coming soon
    }

    #endregion

    #region Memento Pattern Implementation

    /// <summary>
    /// Creates a memento containing a snapshot of the current state
    /// </summary>
    public PaymentMemento CreateMemento(string metadata = "")
    {
        return new PaymentMemento(this, metadata);
    }

    /// <summary>
    /// Restores the state from a memento
    /// </summary>
    public Result RestoreFromMemento(PaymentMemento memento)
    {
        try
        {
            var state = JsonSerializer.Deserialize<PaymentState>(memento.GetState());

            if (state == null)
            {
                return Result.Failure(
                    DomainErrors.Payment.DeserializationFailed);
            }

            // Restore properties from memento
            Amount = Amount.Create(state.Amount).Value;
            Status = state.Status;
            UserId = state.UserId;
            ScheduledDate = state.ScheduledDate;
            TransactionId = state.TransactionId;
            RefundTransactionId = state.RefundTransactionId;
            ProviderName = state.ProviderName;
            ProcessedDate = state.ProcessedDate;
            RefundedDate = state.RefundedDate;
            CancelledDate = state.CancelledDate;
            FailureReason = state.FailureReason;

            if (!string.IsNullOrEmpty(state.PaymentMethod))
            {
                PaymentMethod = Enum.Parse<PaymentMethod>(state.PaymentMethod);
            }
            else
            {
                PaymentMethod = null;
            }

            CreatedOnUtc = state.CreatedOnUtc;
            ModifiedOnUtc = DateTime.UtcNow; // Update modified time

            // Raise domain event for state restoration
            RaiseDomainEvent(new PaymentStateRestoredDomainEvent(
                Guid.NewGuid(),
                Id));

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(
                DomainErrors.Payment.RestoreStateError(ex.Message));
        }
    }

    // Private class to help with deserialization
    private class PaymentState
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentStatus Status { get; set; }
        public Guid UserId { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string TransactionId { get; set; }
        public string RefundTransactionId { get; set; }
        public string ProviderName { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public DateTime? RefundedDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string FailureReason { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
    }

    #endregion
}
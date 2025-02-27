using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Domain.Builders.Payments;

/// <summary>
/// Builder for creating Payment entities
/// </summary>
/// <remarks>
/// Constructor with required parameters
/// </remarks>
public class PaymentBuilder(Guid userId, decimal amount) : IBuilder<Payment>
{
    #region Private Properties

    private Amount _amount = Amount.Create(amount).Value;

    // Optional parameters with default values
    private PaymentStatus _status = PaymentStatus.Pending;
    private DateTime? _scheduledDate;
    private PaymentMethod _paymentMethod;
    private string _providerName;

    #endregion

    #region Building Blocks

    /// <summary>
    /// Sets the payment status
    /// </summary>
    public PaymentBuilder WithStatus(PaymentStatus status)
    {
        _status = status;
        return this;
    }

    /// <summary>
    /// Schedules the payment for a future date
    /// </summary>
    public PaymentBuilder ScheduleFor(DateTime date)
    {
        if (date < DateTime.UtcNow)
        {
            throw new ArgumentException("Scheduled date must be in the future");
        }

        _scheduledDate = date;
        _status = PaymentStatus.Scheduled;
        return this;
    }

    /// <summary>
    /// Sets the payment method
    /// </summary>
    public PaymentBuilder WithPaymentMethod(PaymentMethod paymentMethod)
    {
        _paymentMethod = paymentMethod;
        return this;
    }

    /// <summary>
    /// Sets the payment provider
    /// </summary>
    public PaymentBuilder WithProvider(string providerName)
    {
        _providerName = providerName;
        return this;
    }

    #endregion

    #region Build Construct

    /// <summary>
    /// Builds the Payment instance
    /// </summary>
    public Payment Build()
    {
        var payment = Payment.Create(
            Guid.NewGuid(),
            _amount,
            _status,
            userId,
            _scheduledDate);

        payment.SetPaymentMethod(_paymentMethod);

        if (!string.IsNullOrEmpty(_providerName))
        {
            payment.SetProvider(_providerName);
        }

        return payment;
    }

    #endregion
}
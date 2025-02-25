namespace PayVerse.Domain.Abstractions.Payments;

/// <summary>
/// Represents a factory for creating payment provider components.
/// </summary>
public interface IPaymentProviderFactory
{
    /// <summary>
    /// Gets the name of the payment provider.
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Creates a payment processor for the specific provider.
    /// </summary>
    /// <returns>A payment processor.</returns>
    IPaymentProcessor CreatePaymentProcessor();

    /// <summary>
    /// Creates a payment transaction manager for the specific provider.
    /// </summary>
    /// <returns>A payment transaction manager.</returns>
    IPaymentTransaction CreateTransactionManager();
}
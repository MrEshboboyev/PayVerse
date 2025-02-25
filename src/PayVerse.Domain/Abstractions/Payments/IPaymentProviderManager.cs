namespace PayVerse.Domain.Abstractions.Payments;

/// <summary>
/// Manages payment providers and factories.
/// </summary>
public interface IPaymentProviderManager
{
    /// <summary>
    /// Gets a payment provider factory by provider name.
    /// </summary>
    /// <param name="providerName">The name of the provider.</param>
    /// <returns>The payment provider factory.</returns>
    IPaymentProviderFactory GetFactory(string providerName);

    /// <summary>
    /// Gets the default payment provider factory.
    /// </summary>
    /// <returns>The default payment provider factory.</returns>
    IPaymentProviderFactory GetDefaultFactory();
}
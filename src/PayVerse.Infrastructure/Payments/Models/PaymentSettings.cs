// File: PayVerse.Infrastructure/Payments/Models/PaymentSettings.cs
namespace PayVerse.Infrastructure.Payments.Models;

/// <summary>
/// Settings for payment configuration.
/// </summary>
public class PaymentSettings
{
    /// <summary>
    /// Gets or sets the default payment provider.
    /// </summary>
    public string DefaultProvider { get; set; } = "Stripe";
}
namespace PayVerse.Application.Payments.Factories;

public class PaymentProcessorOptions
{
    public bool EnableValidation { get; set; } = true;
    public bool EnableLogging { get; set; } = true;
    public bool EnableNotifications { get; set; } = true;
    public bool EnableFraudDetection { get; set; } = true;
}

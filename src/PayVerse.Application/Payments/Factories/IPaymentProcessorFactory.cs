using PayVerse.Domain.Decorators.Payments;

namespace PayVerse.Application.Payments.Factories;

public interface IPaymentProcessorFactory
{
    IPaymentProcessor CreatePaymentProcessor(PaymentProcessorOptions options);
}

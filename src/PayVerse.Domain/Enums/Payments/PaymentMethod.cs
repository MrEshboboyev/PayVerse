namespace PayVerse.Domain.Enums.Payments;

public enum PaymentMethod
{
    CreditCard = 10,
    DebitCard = 20,
    PayPal = 30,
    Stripe = 40,
    BankTransfer = 50,
    Cash = 60,
    Other = 70,
    VirtualAccount = 80
}

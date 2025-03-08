using PayVerse.Domain.Strategies.ValueObjects;
using System.Text.Json;

namespace PayVerse.Domain.Entities.Payments;

/// <summary>
/// Enhanced Payment entity with support for payment method details
/// </summary>
public class PaymentMethodEntity
{
    public PaymentMethodType Type { get; private set; }
    private readonly string _serializedDetails;

    private PaymentMethodEntity(PaymentMethodType type, string serializedDetails)
    {
        Type = type;
        _serializedDetails = serializedDetails;
    }

    public WalletDetails GetWalletDetails()
    {
        if (Type != PaymentMethodType.Wallet)
            throw new InvalidOperationException("Payment method is not a wallet payment.");

        return JsonSerializer.Deserialize<WalletDetails>(_serializedDetails);
    }

    public static PaymentMethodEntity CreateWalletPayment(Guid walletId)
    {
        var details = new WalletDetails
        {
            WalletId = walletId
        };

        return new PaymentMethodEntity(
            PaymentMethodType.Wallet,
            JsonSerializer.Serialize(details));
    }
}

public class WalletDetails
{
    public Guid WalletId { get; set; }
}
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Domain.Builders.Wallets;

/// <summary>
/// DTO for initial wallet transactions used in the builder
/// </summary>
public class InitialWalletTransactionDto(decimal amount,
                                        string description)
{
    public Amount Amount { get; } = Amount.Create(amount).Value;
    public string Description { get; } = description;
}

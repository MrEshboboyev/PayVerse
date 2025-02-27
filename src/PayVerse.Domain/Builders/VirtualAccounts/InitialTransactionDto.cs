using PayVerse.Domain.ValueObjects;

namespace PayVerse.Domain.Builders.VirtualAccounts;

/// <summary>
/// DTO for initial transactions used in the builder
/// </summary>
public class InitialTransactionDto(decimal amount,
                                   string description)
{
    public Amount Amount { get; } = Amount.Create(amount).Value;
    public string Description { get; } = description;
}

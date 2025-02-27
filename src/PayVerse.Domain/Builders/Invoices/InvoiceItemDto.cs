using PayVerse.Domain.ValueObjects;

namespace PayVerse.Domain.Builders.Invoices;

/// <summary>
/// DTO for invoice items used in the builder
/// </summary>
public class InvoiceItemDto(string description,
                            decimal amount)
{
    public string Description { get; } = description;
    
    // handle currency - coming soon
    public Amount Amount { get; } = Amount.Create(amount).Value;
}

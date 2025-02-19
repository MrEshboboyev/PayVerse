namespace PayVerse.Domain.Reports;

// DataPoint could be a simple value object to represent financial data
public class DataPoint(decimal value, string description)
{
    public decimal Value { get; } = value;
    public string Description { get; } = description;
}

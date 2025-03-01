using PayVerse.Domain.Visitors.Reports;

namespace PayVerse.Domain.Composites.Reports;

/// <summary>
/// Leaf class in the Composite pattern - represents individual financial data entries
/// </summary>
public class FinancialEntry(string name, decimal amount) : IReportComponent
{
    public string Name { get; } = name;
    public decimal Amount { get; } = amount;

    public decimal GetTotal() => Amount;

    // Leaf nodes don't implement these methods meaningfully
    public void Add(IReportComponent component) =>
        throw new NotSupportedException("Cannot add to a leaf node");

    public void Remove(IReportComponent component) =>
        throw new NotSupportedException("Cannot remove from a leaf node");

    public IReportComponent GetChild(int index) =>
        throw new NotSupportedException("Leaf nodes have no children");

    public void Accept(IReportVisitor visitor) => visitor.Visit(this);
}

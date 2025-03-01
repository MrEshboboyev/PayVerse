using PayVerse.Domain.Visitors.Reports;

namespace PayVerse.Domain.Composites.Reports;

/// <summary>
/// Composite class in the Composite pattern - represents a category of financial data that can contain
/// both individual entries and other categories
/// </summary>
public class FinancialCategory(string name) : IReportComponent
{
    private readonly List<IReportComponent> _children = [];

    public string Name { get; } = name;

    public decimal GetTotal()
    {
        decimal total = 0;
        foreach (var child in _children)
        {
            total += child.GetTotal();
        }
        return total;
    }

    public void Add(IReportComponent component)
    {
        _children.Add(component);
    }

    public void Remove(IReportComponent component)
    {
        _children.Remove(component);
    }

    public IReportComponent GetChild(int index)
    {
        return _children[index];
    }

    public IReadOnlyCollection<IReportComponent> GetChildren() => _children.AsReadOnly();

    public void Accept(IReportVisitor visitor) => visitor.Visit(this);
}
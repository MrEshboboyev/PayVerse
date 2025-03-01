using PayVerse.Domain.Visitors.Reports;

namespace PayVerse.Domain.Composites.Reports;

/// <summary>
/// The Component interface for the Composite pattern, representing financial report components
/// </summary>
public interface IReportComponent
{
    string Name { get; }
    decimal GetTotal();
    void Add(IReportComponent component);
    void Remove(IReportComponent component);
    IReportComponent GetChild(int index);
    void Accept(IReportVisitor visitor);
}

using PayVerse.Domain.Composites.Reports;

namespace PayVerse.Domain.Visitors.Reports;

/// <summary>
/// Visitor interface for processing report components
/// </summary>
public interface IReportVisitor
{
    void Visit(FinancialEntry entry);
    void Visit(FinancialCategory category);
}

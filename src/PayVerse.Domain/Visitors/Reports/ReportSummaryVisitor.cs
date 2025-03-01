using PayVerse.Domain.Composites.Reports;

namespace PayVerse.Domain.Visitors.Reports;

/// <summary>
/// Concrete visitor for generating report summaries
/// </summary>
public class ReportSummaryVisitor : IReportVisitor
{
    private readonly List<string> _summaries = new();
    private int _indentationLevel = 0;

    public IReadOnlyList<string> GetSummaries() => _summaries.AsReadOnly();

    public void Visit(FinancialEntry entry)
    {
        var indentation = new string(' ', _indentationLevel * 2);
        _summaries.Add($"{indentation}{entry.Name}: {entry.Amount:C}");
    }

    public void Visit(FinancialCategory category)
    {
        var indentation = new string(' ', _indentationLevel * 2);
        _summaries.Add($"{indentation}{category.Name}: {category.GetTotal():C}");

        _indentationLevel++;
        foreach (var child in ((FinancialCategory)category).GetChildren())
        {
            child.Accept(this);
        }
        _indentationLevel--;
    }
}

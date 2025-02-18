using PayVerse.Domain.Entities.Reports;

namespace PayVerse.Domain.Prototypes.Reports;

// ✅ Benefits:
// Useful when exporting reports in multiple formats without re-processing data.
// Prevents accidental mutations to the original financial report.
public class FinancialReportPrototype(FinancialReport report) : ICloneable
{
    public FinancialReport Report { get; private set; } = report;

    public object Clone()
    {
        return new FinancialReportPrototype(FinancialReport.Create(
            Report.Id,
            //DateTime.UtcNow, // Keep timestamp updated
            Report.Period,
            Report.Type,
            Report.FileType,
            Report.GeneratedBy));
    }
}

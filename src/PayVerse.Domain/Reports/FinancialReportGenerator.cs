namespace PayVerse.Domain.Reports;

// ✅ Subclasses implement specific behaviors.

public abstract class CompositeFinancialReportGenerator
{
    // Template method
    public Report GenerateReport()
    {
        var data = CollectData();
        var processedData = ProcessData(data);
        var formattedReport = FormatReport(processedData);
        return FinalizeReport(formattedReport);
    }

    // Steps to be implemented by subclasses
    protected abstract List<DataPoint> CollectData();
    protected abstract List<DataPoint> ProcessData(List<DataPoint> data);
    protected abstract string FormatReport(List<DataPoint> processedData);
    protected abstract Report FinalizeReport(string formattedReport);
}

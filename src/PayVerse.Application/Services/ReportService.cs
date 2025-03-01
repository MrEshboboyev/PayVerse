using PayVerse.Domain.Reports;

namespace PayVerse.Application.Services;

public class ReportService
{
    public Report GenerateDailyReport()
    {
        var generator = new DailyCompositeFinancialReportGenerator();
        return generator.GenerateReport();
    }

    public Report GenerateMonthlyReport()
    {
        var generator = new MonthlyCompositeFinancialReportGenerator();
        return generator.GenerateReport();
    }
}
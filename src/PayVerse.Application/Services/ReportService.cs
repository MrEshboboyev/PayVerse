using PayVerse.Domain.Reports;

namespace PayVerse.Application.Services;

public class ReportService
{
    public Report GenerateDailyReport()
    {
        var generator = new DailyFinancialReportGenerator();
        return generator.GenerateReport();
    }

    public Report GenerateMonthlyReport()
    {
        var generator = new MonthlyFinancialReportGenerator();
        return generator.GenerateReport();
    }
}
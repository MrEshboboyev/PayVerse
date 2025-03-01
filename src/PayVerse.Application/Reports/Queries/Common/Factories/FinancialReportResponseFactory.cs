using PayVerse.Application.Reports.Queries.Common.Responses;
using PayVerse.Domain.Entities.Reports;

namespace PayVerse.Application.Reports.Queries.Common.Factories;

public static class CompositeFinancialReportResponseFactory
{
    public static CompositeFinancialReportResponse Create(CompositeFinancialReport report)
    {
        var period = ReportPeriodResponseFactory.Create(report.Period);

        return new CompositeFinancialReportResponse(
            period,
            report.Type,
            report.Status,
            report.FilePath,
            report.GeneratedBy,
            report.GeneratedAt,
            report.CreatedOnUtc,
            report.ModifiedOnUtc);
    }
}
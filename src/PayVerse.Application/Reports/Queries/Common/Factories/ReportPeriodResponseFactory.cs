using PayVerse.Application.Reports.Queries.Common.Responses;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Application.Reports.Queries.Common.Factories;

public static class ReportPeriodResponseFactory
{
    public static ReportPeriodResponse Create(ReportPeriod period)
    {
        return new ReportPeriodResponse(
            period.StartDate,
            period.EndDate);
    }
}
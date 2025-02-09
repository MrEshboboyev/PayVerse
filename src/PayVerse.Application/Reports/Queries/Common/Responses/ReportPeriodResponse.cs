namespace PayVerse.Application.Reports.Queries.Common.Responses;

public sealed record ReportPeriodResponse(
    DateOnly StartDate,
    DateOnly EndDate);
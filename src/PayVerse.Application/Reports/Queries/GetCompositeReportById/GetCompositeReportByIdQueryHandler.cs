using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Factories;
using PayVerse.Application.Reports.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Reports.Queries.GetCompositeReportById;

public sealed class GetCompositeReportByIdQueryHandler(
    ICompositeFinancialReportRepository reportRepository) : IQueryHandler<GetCompositeReportByIdQuery, CompositeReportResponse>
{
    public async Task<Result<CompositeReportResponse>> Handle(
        GetCompositeReportByIdQuery query,
        CancellationToken cancellationToken)
    {
        var report = await reportRepository.GetByIdAsync(query.Id, cancellationToken);

        if (report is null)
        {
            return Result.Failure<CompositeReportResponse>(
                DomainErrors.CompositeFinancialReport.NotFound(query.Id));
        }

        var response = new CompositeReportResponse(
            report.Id,
            report.Title.Value,
            ReportPeriodResponseFactory.Create(report.Period),
            report.Type,
            report.FileType,
            report.Status,
            report.GetTotalAmount(),
            report.GenerateSummary(),
            report.FilePath,
            report.GeneratedAt);

        return Result.Success(response);
    }
}
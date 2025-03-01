using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Factories;
using PayVerse.Application.Reports.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Application.Reports.Queries.GetCompositeFinancialReportsByPeriod;

internal sealed class GetCompositeFinancialReportsByPeriodQueryHandler(
    ICompositeFinancialReportRepository financialReportRepository) : IQueryHandler<GetCompositeFinancialReportsByPeriodQuery, CompositeFinancialReportListResponse>
{
    public async Task<Result<CompositeFinancialReportListResponse>> Handle(
        GetCompositeFinancialReportsByPeriodQuery request,
        CancellationToken cancellationToken)
    {
        var (startDate, endDate) = request;
        
        #region Prepare value object
        
        var periodResult = ReportPeriod.Create(startDate, endDate);
        if (periodResult.IsFailure)
        {
            return Result.Failure<CompositeFinancialReportListResponse>(
                periodResult.Error);
        }
        
        #endregion
        
        var reports = await financialReportRepository.GetByPeriodAsync(
            periodResult.Value,
            cancellationToken);
        
        var response = new CompositeFinancialReportListResponse(
            reports.Select(CompositeFinancialReportResponseFactory.Create)
                .ToList().AsReadOnly());

        return Result.Success(response);
    }
}
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Factories;
using PayVerse.Application.Reports.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Application.Reports.Queries.GetFinancialReportsByPeriod;

internal sealed class GetFinancialReportsByPeriodQueryHandler(
    IFinancialReportRepository financialReportRepository) : IQueryHandler<GetFinancialReportsByPeriodQuery, FinancialReportListResponse>
{
    public async Task<Result<FinancialReportListResponse>> Handle(
        GetFinancialReportsByPeriodQuery request,
        CancellationToken cancellationToken)
    {
        var (startDate, endDate) = request;
        
        #region Prepare value object
        
        var periodResult = ReportPeriod.Create(startDate, endDate);
        if (periodResult.IsFailure)
        {
            return Result.Failure<FinancialReportListResponse>(
                periodResult.Error);
        }
        
        #endregion
        
        var reports = await financialReportRepository.GetByPeriodAsync(
            periodResult.Value,
            cancellationToken);
        
        var response = new FinancialReportListResponse(
            reports.Select(FinancialReportResponseFactory.Create)
                .ToList().AsReadOnly());

        return Result.Success(response);
    }
}
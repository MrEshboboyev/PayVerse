using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Factories;
using PayVerse.Application.Reports.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Reports.Queries.GetFinancialReportById;

internal sealed class GetFinancialReportByIdQueryHandler(
    IFinancialReportRepository financialReportRepository) : IQueryHandler<GetFinancialReportByIdQuery, FinancialReportResponse>
{
    public async Task<Result<FinancialReportResponse>> Handle(
        GetFinancialReportByIdQuery request,
        CancellationToken cancellationToken)
    {
        var reportId = request.ReportId;
        
        #region Get this Report
        
        var report = await financialReportRepository.GetByIdAsync(
            reportId,
            cancellationToken);
        if (report is null)
        {
            return Result.Failure<FinancialReportResponse>(
                DomainErrors.FinancialReport.NotFound(reportId));
        }
        
        #endregion
        
        #region Prepare response
        
        var response = FinancialReportResponseFactory.Create(report);
        
        #endregion
        
        return Result.Success(response);
    }
}
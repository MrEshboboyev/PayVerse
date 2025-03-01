using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Factories;
using PayVerse.Application.Reports.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Reports.Queries.GetCompositeFinancialReportById;

internal sealed class GetCompositeFinancialReportByIdQueryHandler(
    ICompositeFinancialReportRepository financialReportRepository) : IQueryHandler<GetCompositeFinancialReportByIdQuery, CompositeFinancialReportResponse>
{
    public async Task<Result<CompositeFinancialReportResponse>> Handle(
        GetCompositeFinancialReportByIdQuery request,
        CancellationToken cancellationToken)
    {
        var reportId = request.ReportId;
        
        #region Get this Report
        
        var report = await financialReportRepository.GetByIdAsync(
            reportId,
            cancellationToken);
        if (report is null)
        {
            return Result.Failure<CompositeFinancialReportResponse>(
                DomainErrors.CompositeFinancialReport.NotFound(reportId));
        }
        
        #endregion
        
        #region Prepare response
        
        var response = CompositeFinancialReportResponseFactory.Create(report);
        
        #endregion
        
        return Result.Success(response);
    }
}
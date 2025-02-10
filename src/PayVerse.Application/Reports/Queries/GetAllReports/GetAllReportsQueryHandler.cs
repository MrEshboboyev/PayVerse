using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Factories;
using PayVerse.Application.Reports.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Reports.Queries.GetAllReports;

internal sealed class GetAllReportsQueryHandler(
    IFinancialReportRepository financialReportRepository) : IQueryHandler<GetAllReportsQuery, FinancialReportListResponse>
{
    public async Task<Result<FinancialReportListResponse>> Handle(
        GetAllReportsQuery request,
        CancellationToken cancellationToken)
    {
        var allReports = await financialReportRepository.GetAllAsync(cancellationToken);

        var response = new FinancialReportListResponse(
            allReports.Select(FinancialReportResponseFactory.Create)
                .ToList().AsReadOnly());
        
        return Result.Success(response);
    }
}
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Factories;
using PayVerse.Application.Reports.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Reports.Queries.GetAllReports;

internal sealed class GetAllReportsQueryHandler(
    ICompositeFinancialReportRepository financialReportRepository) : IQueryHandler<GetAllReportsQuery, CompositeFinancialReportListResponse>
{
    public async Task<Result<CompositeFinancialReportListResponse>> Handle(
        GetAllReportsQuery request,
        CancellationToken cancellationToken)
    {
        var allReports = await financialReportRepository.GetAllAsync(cancellationToken);

        var response = new CompositeFinancialReportListResponse(
            allReports.Select(CompositeFinancialReportResponseFactory.Create)
                .ToList().AsReadOnly());
        
        return Result.Success(response);
    }
}
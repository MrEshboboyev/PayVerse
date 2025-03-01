using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Factories;
using PayVerse.Application.Reports.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Reports.Queries.GetCompositeFinancialReportsByUser;

internal sealed class GetCompositeFinancialReportsByUserQueryHandler(
    ICompositeFinancialReportRepository financialReportRepository) : IQueryHandler<GetCompositeFinancialReportsByUserQuery, CompositeFinancialReportListResponse>
{
    public async Task<Result<CompositeFinancialReportListResponse>> Handle(
        GetCompositeFinancialReportsByUserQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        
        var reports = await financialReportRepository.GetByUserIdAsync(
            userId,
            cancellationToken);

        var response = new CompositeFinancialReportListResponse(
            reports.Select(CompositeFinancialReportResponseFactory.Create)
                .ToList().AsReadOnly());
        
        return Result.Success(response);
    }
}
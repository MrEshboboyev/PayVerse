using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Factories;
using PayVerse.Application.Reports.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Reports.Queries.GetFinancialReportsByUser;

internal sealed class GetFinancialReportsByUserQueryHandler(
    IFinancialReportRepository financialReportRepository) : IQueryHandler<GetFinancialReportsByUserQuery, FinancialReportListResponse>
{
    public async Task<Result<FinancialReportListResponse>> Handle(
        GetFinancialReportsByUserQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        
        var reports = await financialReportRepository.GetByUserIdAsync(
            userId,
            cancellationToken);

        var response = new FinancialReportListResponse(
            reports.Select(FinancialReportResponseFactory.Create)
                .ToList().AsReadOnly());
        
        return Result.Success(response);
    }
}
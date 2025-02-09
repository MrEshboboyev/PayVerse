using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Reports.Commands.FailFinancialReport;

internal sealed class FailFinancialReportCommandHandler(
    IFinancialReportRepository financialReportRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<FailFinancialReportCommand>
{
    public async Task<Result> Handle(
        FailFinancialReportCommand request,
        CancellationToken cancellationToken)
    {
        var reportId = request.ReportId;
        
        #region Get this report
        
        var report = await financialReportRepository.GetByIdAsync(
            reportId,
            cancellationToken);
        if (report is null)
        {
            return Result.Failure(
                DomainErrors.FinancialReport.NotFound(reportId));
        }
        
        #endregion
        
        #region Mark as failed report
        
        var result = report.MarkAsFailed();
        if (result.IsFailure)
            return result;
        
        #endregion

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

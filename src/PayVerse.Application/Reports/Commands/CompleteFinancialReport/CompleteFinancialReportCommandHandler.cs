using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Reports.Commands.CompleteFinancialReport;

internal sealed class CompleteFinancialReportCommandHandler(
    IFinancialReportRepository financialReportRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CompleteFinancialReportCommand>
{
    public async Task<Result> Handle(
        CompleteFinancialReportCommand request,
        CancellationToken cancellationToken)
    {
        var (reportId, filePath) = request;
        
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
        
        #region Mark as complete this report

        var result = report.MarkAsCompleted(filePath);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }
        
        #endregion

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Application.Reports.Commands.GenerateFinancialReport;

internal sealed class GenerateFinancialReportCommandHandler(
    IFinancialReportRepository financialReportRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<GenerateFinancialReportCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        GenerateFinancialReportCommand request,
        CancellationToken cancellationToken)
    {
        var (userId, startDate, endDate, type) = request;
        
        #region Prepare value objects
        
        var periodResult = ReportPeriod.Create(startDate, endDate);
        if (periodResult.IsFailure)
        {
            return Result.Failure<Guid>(periodResult.Error);
        }
        
        #endregion

        #region Report data
        
        var report = FinancialReport.Create(
            Guid.NewGuid(),
            periodResult.Value,
            type,
            userId);
        
        #endregion

        await financialReportRepository.AddAsync(report, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(report.Id);
    }
}
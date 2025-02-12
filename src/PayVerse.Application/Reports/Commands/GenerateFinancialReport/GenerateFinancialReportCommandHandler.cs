using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Services;
using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Application.Reports.Commands.GenerateFinancialReport;

internal sealed class GenerateFinancialReportCommandHandler(
    IFinancialReportRepository financialReportRepository,
    IReportGeneratorFactory reportGeneratorFactory,
    IUnitOfWork unitOfWork) : ICommandHandler<GenerateFinancialReportCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        GenerateFinancialReportCommand request,
        CancellationToken cancellationToken)
    {
        var (userId, startDate, endDate, type, fileType) = request;
        
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
            fileType,
            userId);
        
        #endregion
        
        #region Generate report
        
        // Generate the report
        var generator = reportGeneratorFactory.CreateReportGenerator(report.FileType);
        var filePath = await generator.GenerateAsync(report, cancellationToken);

        // Update report with file path and mark as completed
        var updateResult = report.MarkAsCompleted(filePath);
        if (updateResult.IsFailure)
        {
            return Result.Failure<Guid>(
                updateResult.Error);
        }
        
        #endregion

        await financialReportRepository.AddAsync(report, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(report.Id);
    }
}
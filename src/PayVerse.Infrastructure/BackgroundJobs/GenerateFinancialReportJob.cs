using Microsoft.Extensions.Logging;
using PayVerse.Application.Reports.Services;
using PayVerse.Domain.Enums.Reports;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Reports;
using Quartz;

namespace PayVerse.Infrastructure.BackgroundJobs;

/// <summary>
/// Background job for generating financial reports.
/// </summary>
[DisallowConcurrentExecution]
public sealed class GenerateFinancialReportJob(
    IFinancialReportRepository financialReportRepository,
    IReportGeneratorFactory reportGeneratorFactory,
    // IEmailService emailService,
    IUnitOfWork unitOfWork,
    ILogger<GenerateFinancialReportJob> logger) : IJob
{
    /// <summary>
    /// Executes the job to generate a financial report.
    /// </summary>
    /// <param name="context">The job execution context.</param>
    public async Task Execute(IJobExecutionContext context)
    {
        var reportId = context.JobDetail.JobDataMap.GetGuid("ReportId");
        if (reportId == Guid.Empty)
        {
            logger.LogWarning("ReportId is missing. Skipping job execution.");
            return;
        }

        logger.LogInformation("Starting report generation for ReportId: {ReportId}", reportId);

        // Fetch the pending report from the repository
        var report = await financialReportRepository.GetByIdAsync(reportId, context.CancellationToken);
        if (report is null || report.Status != ReportStatus.Pending)
        {
            logger.LogWarning("Report not found or already processed. ReportId: {ReportId}", reportId);
            return;
        }

        try
        {
            // Get the appropriate report generator
            var generator = reportGeneratorFactory.CreateReportGenerator(report.FileType);

            // Generate the report file
            var filePath = await generator.GenerateAsync(report, context.CancellationToken);

            // Mark report as completed
            var result = report.MarkAsCompleted(filePath);
            if (result.IsFailure)
            {
                logger.LogError("Failed to mark report as completed. ReportId: {ReportId}", reportId);
                return;
            }

            // Persist changes
            await unitOfWork.SaveChangesAsync(context.CancellationToken);

            // // Send email notification
            // await emailService.SendReportEmailAsync(report.GeneratedBy, filePath, context.CancellationToken);

            logger.LogInformation("Report generated successfully. ReportId: {ReportId}", reportId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while generating report. ReportId: {ReportId}", reportId);

            // Mark report as failed
            report.MarkAsFailed();
            await unitOfWork.SaveChangesAsync(context.CancellationToken);
        }
    }
}

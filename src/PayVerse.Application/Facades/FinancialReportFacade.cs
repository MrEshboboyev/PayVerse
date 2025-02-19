using PayVerse.Application.Interfaces;
using PayVerse.Domain.Enums.Reports;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Application.Facades;

// ✅ Simplifies the process of generating, saving, and emailing financial reports.

// ✅ Future Benefits:
// Prevents code duplication by providing a unified interface.
// Easily extends to support PDF exports, API integrations.

public class FinancialReportFacade(IFinancialReportService reportService)
{
    private readonly IFinancialReportService _reportService =
        reportService ?? throw new ArgumentNullException(nameof(reportService));

    /// <summary>
    /// Orchestrates the generation, saving, and emailing of a financial report.
    /// </summary>
    /// <param name="userId">The ID of the user requesting the report.</param>
    /// <param name="email">The email address to send the report to.</param>
    /// <param name="reportType">The type of report to generate.</param>
    /// <param name="period">The period for which the report is to be generated.</param>
    /// <returns>A Result indicating success or failure of the operation.</returns>
    public async Task<Result> GenerateAndSendReportAsync(
        Guid userId, 
        string email, 
        ReportType reportType, 
        ReportPeriod period)
    {
        Console.WriteLine("[Facade] Starting financial report process...");

        #region Generate Report

        var generateResult = await _reportService.GenerateReportAsync(
            userId, 
            period, 
            reportType, 
            FileType.Pdf);
        if (generateResult.IsFailure)
        {
            Console.WriteLine($"[Facade] Error generating report: {generateResult.Error}");
            return Result.Failure(generateResult.Error);
        }

        var report = generateResult.Value;

        #endregion

        #region Save Report

        var saveResult = await _reportService.SaveReportAsync(
            report, 
            "path/to/save/report.pdf"); // Placeholder path
        if (saveResult.IsFailure)
        {
            Console.WriteLine($"[Facade] Error saving report: {saveResult.Error}");
            return Result.Failure(saveResult.Error);
        }

        #endregion

        #region Send Report

        var sendResult = await _reportService.SendReportByEmailAsync(
            report, 
            email);
        if (sendResult.IsFailure)
        {
            Console.WriteLine($"[Facade] Error sending report via email: {sendResult.Error}");
            return Result.Failure(sendResult.Error);
        }

        #endregion

        Console.WriteLine("[Facade] Financial report process completed successfully.");
        return Result.Success();
    }
}
using PayVerse.Application.Interfaces;
using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Enums.Reports;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Infrastructure.Services;

public class FinancialReportService(
    IFinancialReportRepository reportRepository, 
    IUnitOfWork unitOfWork, 
    IEmailService emailService) : IFinancialReportService
{
    private readonly IFinancialReportRepository _reportRepository = reportRepository;
    private readonly IEmailService _emailService = emailService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<FinancialReport>> GenerateReportAsync(
        Guid userId, 
        ReportPeriod period, 
        ReportType type, 
        FileType fileType)
    {
        Console.WriteLine($"[Generating Report] Fetching transactions for user {userId} and period {period}...");

        var report = FinancialReport.Create(
            Guid.NewGuid(),
            period,
            type,
            fileType,
            userId);

        await _reportRepository.AddAsync(report);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(report);
    }

    public async Task<Result> SaveReportAsync(
        FinancialReport report, 
        string filePath)
    {
        var markResult = report.MarkAsCompleted(filePath);
        if (markResult.IsFailure)
        {
            return markResult;
        }

        Console.WriteLine($"[Saving Report] Report {report.Id} " +
            $"marked as completed and saved successfully.");
        
        await _reportRepository.UpdateAsync(report);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> SendReportByEmailAsync(
        FinancialReport report, 
        string email)
    {
        if (report.Status != ReportStatus.Completed)
        {
            return Result.Failure(
                DomainErrors.FinancialReport.ReportNotCompleted(report.Id)); // write this domain error
        }

        Console.WriteLine($"[Emailing Report] Sending report {report.Id} to {email}...");
        return await _emailService.SendEmailWithAttachment(
            email, 
            "Financial Report",
            $"Please find your financial report attached.File path: {report.FilePath}", 
            report.FilePath);
    }
}
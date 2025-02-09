using FluentValidation;

namespace PayVerse.Application.Reports.Commands.CompleteFinancialReport;

internal class CompleteFinancialReportCommandValidator : AbstractValidator<CompleteFinancialReportCommand>
{
    public CompleteFinancialReportCommandValidator()
    {
        RuleFor(cmd => cmd.ReportId)
            .NotEmpty().WithMessage("Report ID is required.");

        RuleFor(cmd => cmd.FilePath)
            .NotEmpty().WithMessage("File path is required.")
            .Must(IsValidFilePath).WithMessage("File path is not valid.");
        return;

        bool IsValidFilePath(string filePath)
        {
            // Add custom logic to validate file path if needed
            return true;
        }
    }
}
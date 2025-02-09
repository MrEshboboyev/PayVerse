using FluentValidation;

namespace PayVerse.Application.Reports.Commands.FailFinancialReport;

internal class FailFinancialReportCommandValidator : AbstractValidator<FailFinancialReportCommand>
{
    public FailFinancialReportCommandValidator()
    {
        RuleFor(cmd => cmd.ReportId)
            .NotEmpty().WithMessage("Report ID is required.");
    }
}
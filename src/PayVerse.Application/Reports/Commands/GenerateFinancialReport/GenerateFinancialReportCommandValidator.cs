using FluentValidation;

namespace PayVerse.Application.Reports.Commands.GenerateFinancialReport;

internal class GenerateFinancialReportCommandValidator : AbstractValidator<GenerateFinancialReportCommand>
{
    public GenerateFinancialReportCommandValidator()
    {
        RuleFor(cmd => cmd.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(cmd => cmd.StartDate)
            .LessThanOrEqualTo(cmd => cmd.EndDate).WithMessage("Start date must be before or equal to the end date.");

        RuleFor(cmd => cmd.EndDate)
            .GreaterThanOrEqualTo(cmd => cmd.StartDate).WithMessage("End date must be after or equal to the start date.");

        RuleFor(cmd => cmd.Type)
            .IsInEnum().WithMessage("Report type is not valid.");
        
        RuleFor(cmd => cmd.FileType)
            .IsInEnum().WithMessage("File type is not valid.");
    }
}

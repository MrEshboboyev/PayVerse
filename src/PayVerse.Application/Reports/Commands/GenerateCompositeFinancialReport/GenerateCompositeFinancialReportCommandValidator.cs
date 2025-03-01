using FluentValidation;

namespace PayVerse.Application.Reports.Commands.GenerateCompositeFinancialReport;

internal class GenerateCompositeFinancialReportCommandValidator : AbstractValidator<GenerateCompositeFinancialReportCommand>
{
    public GenerateCompositeFinancialReportCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId cannot be empty.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title cannot be empty.");

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("StartDate must be before or equal to EndDate.");
    }
}

using FluentValidation;

namespace PayVerse.Application.VirtualAccounts.Commands.AddTransaction;

internal class AddTransactionCommandValidator : AbstractValidator<AddTransactionCommand>
{
    public AddTransactionCommandValidator()
    {
        RuleFor(cmd => cmd.VirtualAccountId)
            .NotEmpty().WithMessage("Virtual account ID is required.");

        RuleFor(cmd => cmd.Amount)
            .GreaterThan(0).WithMessage("Transaction amount must be greater than zero.");

        RuleFor(cmd => cmd.Date)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Transaction date must not be in the future.");

        RuleFor(cmd => cmd.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");
    }
}

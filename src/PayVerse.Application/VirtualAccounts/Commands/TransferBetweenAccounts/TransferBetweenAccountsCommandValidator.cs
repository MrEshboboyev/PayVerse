using FluentValidation;

namespace PayVerse.Application.VirtualAccounts.Commands.TransferBetweenAccounts;

internal class TransferBetweenAccountsCommandValidator : AbstractValidator<TransferBetweenAccountsCommand>
{
    public TransferBetweenAccountsCommandValidator()
    {
        RuleFor(cmd => cmd.FromAccountId)
            .NotEmpty().WithMessage("Source account ID is required.");

        RuleFor(cmd => cmd.ToAccountId)
            .NotEmpty().WithMessage("Destination account ID is required.")
            .NotEqual(cmd => cmd.FromAccountId).WithMessage("Source and destination accounts must be different.");

        RuleFor(cmd => cmd.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(cmd => cmd.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.");
    }
}
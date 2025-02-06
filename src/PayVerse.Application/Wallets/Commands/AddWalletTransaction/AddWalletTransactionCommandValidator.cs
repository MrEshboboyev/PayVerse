using FluentValidation;

namespace PayVerse.Application.Wallets.Commands.AddWalletTransaction;

internal class AddWalletTransactionCommandValidator : AbstractValidator<AddWalletTransactionCommand>
{
    public AddWalletTransactionCommandValidator()
    {
        RuleFor(cmd => cmd.WalletId)
            .NotEmpty().WithMessage("Wallet ID is required.");

        RuleFor(cmd => cmd.Amount)
            .GreaterThan(0).WithMessage("Transaction amount must be greater than zero.");

        RuleFor(cmd => cmd.Date)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Transaction date must not be in the future.");

        RuleFor(cmd => cmd.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");
    }
}

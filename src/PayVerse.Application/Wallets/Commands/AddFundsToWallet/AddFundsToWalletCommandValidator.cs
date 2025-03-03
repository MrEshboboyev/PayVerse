using FluentValidation;

namespace PayVerse.Application.Wallets.Commands.AddFundsToWallet;

internal class AddFundsToWalletCommandValidator : AbstractValidator<AddFundsToWalletCommand>
{
    public AddFundsToWalletCommandValidator()
    {
        RuleFor(cmd => cmd.WalletId)
            .NotEmpty().WithMessage("Wallet ID is required.");

        RuleFor(cmd => cmd.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(cmd => cmd.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.");
    }
}

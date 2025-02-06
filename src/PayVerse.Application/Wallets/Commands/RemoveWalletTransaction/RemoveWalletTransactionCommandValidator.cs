using FluentValidation;

namespace PayVerse.Application.Wallets.Commands.RemoveWalletTransaction;

internal class RemoveWalletTransactionCommandValidator : AbstractValidator<RemoveWalletTransactionCommand>
{
    public RemoveWalletTransactionCommandValidator()
    {
        RuleFor(cmd => cmd.WalletId)
            .NotEmpty().WithMessage("Wallet ID is required.");

        RuleFor(cmd => cmd.TransactionId)
            .NotEmpty().WithMessage("Transaction ID is required.");
    }
}

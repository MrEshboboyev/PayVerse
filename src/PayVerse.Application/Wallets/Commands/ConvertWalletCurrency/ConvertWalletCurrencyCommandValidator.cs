using FluentValidation;

namespace PayVerse.Application.Wallets.Commands.ConvertWalletCurrency;

internal class ConvertWalletCurrencyCommandValidator : AbstractValidator<ConvertWalletCurrencyCommand>
{
    public ConvertWalletCurrencyCommandValidator()
    {
        RuleFor(cmd => cmd.WalletId).NotEmpty();
        RuleFor(cmd => cmd.NewCurrencyCode).NotEmpty();
    }
}
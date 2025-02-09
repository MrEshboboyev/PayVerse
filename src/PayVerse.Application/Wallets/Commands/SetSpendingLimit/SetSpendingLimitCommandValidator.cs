using FluentValidation;

namespace PayVerse.Application.Wallets.Commands.SetSpendingLimit;

internal class SetSpendingLimitCommandValidator : AbstractValidator<SetSpendingLimitCommand>
{
    public SetSpendingLimitCommandValidator()
    {
        RuleFor(cmd => cmd.WalletId).NotEmpty();
        RuleFor(cmd => cmd.SpendingLimit).GreaterThan(0);
    }
}
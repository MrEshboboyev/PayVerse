using FluentValidation;

namespace PayVerse.Application.Wallets.Commands.RedeemLoyaltyPoints;

internal class RedeemLoyaltyPointsCommandValidator : AbstractValidator<RedeemLoyaltyPointsCommand>
{
    public RedeemLoyaltyPointsCommandValidator()
    {
        RuleFor(cmd => cmd.WalletId).NotEmpty();
        RuleFor(cmd => cmd.Points).GreaterThan(0);
    }
}
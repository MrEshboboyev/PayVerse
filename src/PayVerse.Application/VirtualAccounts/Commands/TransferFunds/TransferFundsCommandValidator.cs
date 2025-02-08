using FluentValidation;

namespace PayVerse.Application.VirtualAccounts.Commands.TransferFunds;

internal class TransferFundsCommandValidator : AbstractValidator<TransferFundsCommand>
{
    public TransferFundsCommandValidator()
    {
        RuleFor(cmd => cmd.FromAccountId).NotEmpty();
        RuleFor(cmd => cmd.ToAccountId).NotEmpty();
        RuleFor(cmd => cmd.Amount).GreaterThan(0);
    }
}

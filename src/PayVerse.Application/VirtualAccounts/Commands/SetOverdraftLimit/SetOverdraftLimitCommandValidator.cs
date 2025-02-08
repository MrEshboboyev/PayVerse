using FluentValidation;

namespace PayVerse.Application.VirtualAccounts.Commands.SetOverdraftLimit;

internal class SetOverdraftLimitCommandValidator : AbstractValidator<SetOverdraftLimitCommand>
{
    public SetOverdraftLimitCommandValidator()
    {
        RuleFor(cmd => cmd.AccountId).NotEmpty();
        RuleFor(cmd => cmd.OverdraftLimit).GreaterThan(0);
    }
}
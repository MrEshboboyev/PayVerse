using FluentValidation;

namespace PayVerse.Application.VirtualAccounts.Commands.UnfreezeVirtualAccount;

internal class UnfreezeVirtualAccountCommandValidator : AbstractValidator<UnfreezeVirtualAccountCommand>
{
    public UnfreezeVirtualAccountCommandValidator()
    {
        RuleFor(cmd => cmd.AccountId).NotEmpty();
    }
}
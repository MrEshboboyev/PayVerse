using FluentValidation;

namespace PayVerse.Application.VirtualAccounts.Commands.FreezeVirtualAccount;

internal class FreezeVirtualAccountCommandValidator : AbstractValidator<FreezeVirtualAccountCommand>
{
    public FreezeVirtualAccountCommandValidator()
    {
        RuleFor(cmd => cmd.AccountId).NotEmpty();
    }
}
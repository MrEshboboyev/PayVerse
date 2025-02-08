using FluentValidation;

namespace PayVerse.Application.VirtualAccounts.Commands.CloseVirtualAccount;

internal class CloseVirtualAccountCommandValidator : AbstractValidator<CloseVirtualAccountCommand>
{
    public CloseVirtualAccountCommandValidator()
    {
        RuleFor(cmd => cmd.AccountId).NotEmpty();
    }
}
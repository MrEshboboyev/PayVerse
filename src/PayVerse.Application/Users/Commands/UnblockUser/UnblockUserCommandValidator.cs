using FluentValidation;

namespace PayVerse.Application.Users.Commands.UnblockUser;

internal class UnblockUserCommandValidator : AbstractValidator<UnblockUserCommand>
{
    public UnblockUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

using FluentValidation;

namespace PayVerse.Application.Users.Commands.RevokeRoleFromUser;

internal class RevokeRoleFromUserCommandValidator : AbstractValidator<RevokeRoleFromUserCommand>
{
    public RevokeRoleFromUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleId).NotEmpty();
    }
}

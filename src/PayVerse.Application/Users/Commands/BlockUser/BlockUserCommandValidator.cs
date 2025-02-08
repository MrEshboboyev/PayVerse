using FluentValidation;

namespace PayVerse.Application.Users.Commands.BlockUser;

internal class BlockUserCommandValidator : AbstractValidator<BlockUserCommand>
{
    public BlockUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

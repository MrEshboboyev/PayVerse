using FluentValidation;
using PayVerse.Domain.ValueObjects.Users;

namespace PayVerse.Application.Users.Commands.UpdateUser;

internal class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(FirstName.MaxLength);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(LastName.MaxLength);
    }
}

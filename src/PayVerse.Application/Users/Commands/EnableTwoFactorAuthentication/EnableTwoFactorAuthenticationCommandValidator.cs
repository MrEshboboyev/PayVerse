using FluentValidation;

namespace PayVerse.Application.Users.Commands.EnableTwoFactorAuthentication;

internal class EnableTwoFactorAuthenticationCommandValidator : AbstractValidator<EnableTwoFactorAuthenticationCommand>
{
    public EnableTwoFactorAuthenticationCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

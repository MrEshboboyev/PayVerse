using FluentValidation;

namespace PayVerse.Application.Users.Commands.CreateUserAndWallet;

internal class CreateUserAndWalletCommandValidator : AbstractValidator<CreateUserAndWalletCommand>
{
    public CreateUserAndWalletCommandValidator()
    {
        RuleFor(x => x.Email).NotNull();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.RoleId).NotNull();
        RuleFor(x => x.CurrencyCode).NotNull();
    }
}

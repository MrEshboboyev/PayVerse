using FluentValidation;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.Wallets.Commands.CreateWallet;

internal class CreateWalletCommandValidator : AbstractValidator<CreateWalletCommand>
{
    public CreateWalletCommandValidator()
    {
        RuleFor(cmd => cmd.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(cmd => cmd.InitialBalance)
            .GreaterThanOrEqualTo(0).WithMessage("Initial balance must be a positive amount or zero.");

        RuleFor(cmd => cmd.CurrencyCode)
            .NotEmpty().WithMessage("Currency code is required.")
            .Length(Currency.CodeLength).WithMessage($"Currency code must be exactly {Currency.CodeLength} characters.");
    }
}

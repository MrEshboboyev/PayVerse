using FluentValidation;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.VirtualAccounts;

namespace PayVerse.Application.VirtualAccounts.Commands.CreateVirtualAccount;

internal class CreateVirtualAccountCommandValidator : AbstractValidator<CreateVirtualAccountCommand>
{
    public CreateVirtualAccountCommandValidator()
    {
        RuleFor(cmd => cmd.AccountNumber)
            .NotEmpty().WithMessage("Account number is required.")
            .Length(AccountNumber.Length)
            .WithMessage($"Account number must be {AccountNumber.Length} characters.");

        RuleFor(cmd => cmd.CurrencyCode)
            .NotEmpty().WithMessage("Currency code is required.")
            .Length(Currency.CodeLength)
            .WithMessage($"Currency code must be exactly {Currency.CodeLength} characters.");

        RuleFor(cmd => cmd.Balance)
            .GreaterThanOrEqualTo(0).WithMessage("Balance must be a positive amount or zero.");

        RuleFor(cmd => cmd.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}

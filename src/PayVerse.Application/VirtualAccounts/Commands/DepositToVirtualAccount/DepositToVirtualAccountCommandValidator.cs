﻿using FluentValidation;

namespace PayVerse.Application.VirtualAccounts.Commands.DepositToVirtualAccount;

internal class DepositToVirtualAccountCommandValidator : AbstractValidator<DepositToVirtualAccountCommand>
{
    public DepositToVirtualAccountCommandValidator()
    {
        RuleFor(cmd => cmd.AccountId)
            .NotEmpty().WithMessage("Account ID is required.");

        RuleFor(cmd => cmd.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(cmd => cmd.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.");
    }
}

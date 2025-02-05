using FluentValidation;
using PayVerse.Domain.ValueObjects.Invoices;

namespace PayVerse.Application.Invoices.Commands.CreateInvoice;

internal class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(cmd => cmd.InvoiceNumber)
            .NotEmpty().WithMessage("Invoice number is required.")
            .MaximumLength(InvoiceNumber.MaxLength)
            .WithMessage($"Invoice number must not exceed {InvoiceNumber.MaxLength} characters.");

        RuleFor(cmd => cmd.InvoiceDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Invoice date must not be in the future.");

        RuleFor(cmd => cmd.TotalAmount)
            .GreaterThan(0).WithMessage("Total amount must be greater than zero.");

        RuleFor(cmd => cmd.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}

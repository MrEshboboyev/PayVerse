using FluentValidation;

namespace PayVerse.Application.Invoices.Commands.AddInvoiceItem;

internal class AddInvoiceItemCommandValidator : AbstractValidator<AddInvoiceItemCommand>
{
    public AddInvoiceItemCommandValidator()
    {
        RuleFor(cmd => cmd.InvoiceId)
            .NotEmpty().WithMessage("Invoice ID is required.");

        RuleFor(cmd => cmd.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");

        RuleFor(cmd => cmd.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}

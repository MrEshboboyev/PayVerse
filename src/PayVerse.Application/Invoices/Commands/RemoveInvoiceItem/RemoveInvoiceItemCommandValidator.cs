using FluentValidation;

namespace PayVerse.Application.Invoices.Commands.RemoveInvoiceItem;

internal class RemoveInvoiceItemCommandValidator : AbstractValidator<RemoveInvoiceItemCommand>
{
    public RemoveInvoiceItemCommandValidator()
    {
        RuleFor(cmd => cmd.InvoiceId)
            .NotEmpty().WithMessage("Invoice ID is required.");

        RuleFor(cmd => cmd.ItemId)
            .NotEmpty().WithMessage("Item ID is required.");
    }
}

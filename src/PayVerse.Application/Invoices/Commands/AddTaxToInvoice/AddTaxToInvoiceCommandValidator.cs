using FluentValidation;

namespace PayVerse.Application.Invoices.Commands.AddTaxToInvoice;

internal class AddTaxToInvoiceCommandValidator : AbstractValidator<AddTaxToInvoiceCommand>
{
    public AddTaxToInvoiceCommandValidator()
    {
        RuleFor(cmd => cmd.InvoiceId).NotEmpty();
        RuleFor(cmd => cmd.TaxAmount).GreaterThan(0);
    }
}
using FluentValidation;

namespace PayVerse.Application.Invoices.Commands.MarkInvoiceAsPaid;

internal class MarkInvoiceAsPaidCommandValidator : AbstractValidator<MarkInvoiceAsPaidCommand>
{
    public MarkInvoiceAsPaidCommandValidator()
    {
        RuleFor(cmd => cmd.InvoiceId).NotEmpty();
    }
}
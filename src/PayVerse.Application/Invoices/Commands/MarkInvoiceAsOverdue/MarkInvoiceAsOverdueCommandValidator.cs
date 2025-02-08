using FluentValidation;

namespace PayVerse.Application.Invoices.Commands.MarkInvoiceAsOverdue;

internal class MarkInvoiceAsOverdueCommandValidator : AbstractValidator<MarkInvoiceAsOverdueCommand>
{
    public MarkInvoiceAsOverdueCommandValidator()
    {
        RuleFor(cmd => cmd.InvoiceId).NotEmpty();
    }
}

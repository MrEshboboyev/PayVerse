using FluentValidation;

namespace PayVerse.Application.Invoices.Commands.FinalizeInvoice;

internal class FinalizeInvoiceCommandValidator : AbstractValidator<FinalizeInvoiceCommand>
{
    public FinalizeInvoiceCommandValidator()
    {
        RuleFor(cmd => cmd.InvoiceId)
            .NotEmpty().WithMessage("Invoice ID is required.");
    }
}

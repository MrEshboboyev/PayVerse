using FluentValidation;

namespace PayVerse.Application.Invoices.Commands.CancelInvoice;

internal class CancelInvoiceCommandValidator : AbstractValidator<CancelInvoiceCommand>
{
    public CancelInvoiceCommandValidator()
    {
        RuleFor(cmd => cmd.InvoiceId)
            .NotEmpty().WithMessage("Invoice ID is required.");

        RuleFor(cmd => cmd.Reason)
            .NotEmpty().WithMessage("Cancellation reason is required.")
            .MaximumLength(500).WithMessage("Cancellation reason cannot exceed 500 characters.");
    }
}

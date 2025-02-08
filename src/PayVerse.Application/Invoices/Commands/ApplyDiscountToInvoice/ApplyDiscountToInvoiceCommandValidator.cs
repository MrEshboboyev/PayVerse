using FluentValidation;

namespace PayVerse.Application.Invoices.Commands.ApplyDiscountToInvoice;

internal class ApplyDiscountToInvoiceCommandValidator : AbstractValidator<ApplyDiscountToInvoiceCommand>
{
    public ApplyDiscountToInvoiceCommandValidator()
    {
        RuleFor(cmd => cmd.InvoiceId).NotEmpty();
        RuleFor(cmd => cmd.DiscountAmount).GreaterThan(0);
    }
}
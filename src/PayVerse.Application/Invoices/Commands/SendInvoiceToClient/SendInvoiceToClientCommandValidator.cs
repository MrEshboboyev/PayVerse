using FluentValidation;

namespace PayVerse.Application.Invoices.Commands.SendInvoiceToClient;

internal class SendInvoiceToClientCommandValidator : AbstractValidator<SendInvoiceToClientCommand>
{
    public SendInvoiceToClientCommandValidator()
    {
        RuleFor(cmd => cmd.InvoiceId).NotEmpty();
        RuleFor(cmd => cmd.Email).NotEmpty().EmailAddress();
    }
}
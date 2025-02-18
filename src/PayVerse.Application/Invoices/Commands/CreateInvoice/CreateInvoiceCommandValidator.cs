using FluentValidation;

namespace PayVerse.Application.Invoices.Commands.CreateInvoice;

internal class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(cmd => cmd.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(cmd => cmd.Items)
            .NotEmpty().WithMessage("At least one invoice item is required.")
            .ForEach(item =>
            {
                item.SetValidator(new InvoiceItemValidator());
            });
    }
}

internal class InvoiceItemValidator : AbstractValidator<(string Description, decimal Amount)>
{
    public InvoiceItemValidator()
    {
        RuleFor(item => item.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(item => item.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}
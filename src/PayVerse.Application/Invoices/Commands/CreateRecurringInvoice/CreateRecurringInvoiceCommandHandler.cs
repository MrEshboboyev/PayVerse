using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Builders.Invoices;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.Invoices.Commands.CreateRecurringInvoice;

internal sealed class CreateRecurringInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateRecurringInvoiceCommand>
{
    public async Task<Result> Handle(
        CreateRecurringInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var (userId, frequencyInMonths, items) = request;

        #region Create Invoice using Builder

        var invoiceBuilder = new InvoiceBuilder(userId);

        foreach (var item in items)
        {
            var (description, amount) = item;

            var amountResult = Amount.Create(amount);
            if (amountResult.IsFailure)
            {
                return Result.Failure(amountResult.Error);
            }
            invoiceBuilder.AddItem(description, amountResult.Value);
        }

        var invoice = invoiceBuilder.Build();

        var setRecurringResult = invoice.SetRecurring(frequencyInMonths);
        if (setRecurringResult.IsFailure)
        {
            return Result.Failure(setRecurringResult.Error);
        }

        #endregion

        await invoiceRepository.AddAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

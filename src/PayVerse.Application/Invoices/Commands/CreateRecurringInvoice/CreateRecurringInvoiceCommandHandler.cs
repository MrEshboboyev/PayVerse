using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Entities.Invoices;
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

        var invoiceBuilder = Invoice.CreateBuilder(userId);

        foreach (var item in items)
        {
            var (description, amount) = item;

            var amountResult = Amount.Create(amount);
            if (amountResult.IsFailure)
            {
                return Result.Failure(amountResult.Error);
            }
            invoiceBuilder.AddItem(description, amountResult.Value.Value);
        }

        var invoice = invoiceBuilder
            .AsRecurring(frequencyInMonths)
            .Build();

        #endregion

        await invoiceRepository.AddAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

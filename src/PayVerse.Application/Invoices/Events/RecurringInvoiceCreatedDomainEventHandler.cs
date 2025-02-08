using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Invoices;
using PayVerse.Domain.Repositories.Invoices;

namespace PayVerse.Application.Invoices.Events;

internal sealed class RecurringInvoiceCreatedDomainEventHandler(
    IInvoiceRepository invoiceRepository)
    : IDomainEventHandler<RecurringInvoiceCreatedDomainEvent>
{
    public async Task Handle(
        RecurringInvoiceCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        #region Get the Invoice

        var invoice = await invoiceRepository.GetByIdAsync(
            notification.InvoiceId,
            cancellationToken);
        if (invoice is null)
        {
            return;
        }

        #endregion

        Console.WriteLine($"Recurring invoice [ID : {invoice.Id}] created with" +
                          $" frequency of {notification.FrequencyInMonths} months.");
    }
}
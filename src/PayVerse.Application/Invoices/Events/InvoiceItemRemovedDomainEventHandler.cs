using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Invoices;
using PayVerse.Domain.Repositories.Invoices;

namespace PayVerse.Application.Invoices.Events;

internal sealed class InvoiceItemRemovedDomainEventHandler(
    IInvoiceRepository invoiceRepository)
    : IDomainEventHandler<InvoiceItemRemovedDomainEvent>
{
    public async Task Handle(
        InvoiceItemRemovedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        #region Get this invoice
        
        var invoice = await invoiceRepository.GetByIdAsync(
            notification.Id,
            cancellationToken);
        if (invoice is null)
        {
            return;
        }
        
        #endregion

        Console.WriteLine($"Invoice item removed with amount : {notification.Amount}" +
                          $" to this invoice [ID : {invoice.Id}]");
    }
}

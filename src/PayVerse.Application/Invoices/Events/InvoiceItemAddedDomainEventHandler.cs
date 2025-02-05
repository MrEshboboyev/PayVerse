using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Invoices;
using PayVerse.Domain.Repositories.Invoices;

namespace PayVerse.Application.Invoices.Events;

internal sealed class InvoiceItemAddedDomainEventHandler(
    IInvoiceRepository invoiceRepository)
    : IDomainEventHandler<InvoiceItemAddedDomainEvent>
{
    public async Task Handle(
        InvoiceItemAddedDomainEvent notification,
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
        
        #region Get this item from Invoice
        
        var invoiceItem = invoice.GetItemById(notification.ItemId);
        if (invoiceItem is null)
        {
            return;
        }
        
        #endregion

        Console.WriteLine($"Invoice item added with amount : {invoiceItem.Amount.Value}" +
                          $" to this invoice [ID : {invoice.Id}]");
    }
}

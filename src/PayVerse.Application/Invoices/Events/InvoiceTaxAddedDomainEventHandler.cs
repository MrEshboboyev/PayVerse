using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Invoices;
using PayVerse.Domain.Repositories.Invoices;

namespace PayVerse.Application.Invoices.Events;

internal sealed class InvoiceTaxAddedDomainEventHandler(
    IInvoiceRepository invoiceRepository)
    : IDomainEventHandler<InvoiceTaxAddedDomainEvent>
{
    public async Task Handle(
        InvoiceTaxAddedDomainEvent notification,
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

        Console.WriteLine($"Tax of amount : {notification.TaxAmount} added" +
                          $" to invoice [ID : {invoice.Id}]");
    }
}
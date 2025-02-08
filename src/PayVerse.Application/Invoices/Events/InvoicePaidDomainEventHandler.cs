using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Invoices;
using PayVerse.Domain.Repositories.Invoices;

namespace PayVerse.Application.Invoices.Events;

internal sealed class InvoicePaidDomainEventHandler(
    IInvoiceRepository invoiceRepository)
    : IDomainEventHandler<InvoicePaidDomainEvent>
{
    public async Task Handle(
        InvoicePaidDomainEvent notification,
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

        Console.WriteLine($"Invoice [ID : {invoice.Id}] has been marked as paid.");
    }
}
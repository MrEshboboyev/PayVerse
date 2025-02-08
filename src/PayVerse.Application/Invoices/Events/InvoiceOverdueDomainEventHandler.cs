using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Invoices;
using PayVerse.Domain.Repositories.Invoices;

namespace PayVerse.Application.Invoices.Events;

internal sealed class InvoiceOverdueDomainEventHandler(
    IInvoiceRepository invoiceRepository)
    : IDomainEventHandler<InvoiceOverdueDomainEvent>
{
    public async Task Handle(
        InvoiceOverdueDomainEvent notification,
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

        Console.WriteLine($"Invoice [ID : {invoice.Id}] has been marked as overdue.");
    }
}
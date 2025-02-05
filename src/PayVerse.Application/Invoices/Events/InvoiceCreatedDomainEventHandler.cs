using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Invoices;
using PayVerse.Domain.Repositories.Invoices;

namespace PayVerse.Application.Invoices.Events;

internal sealed class InvoiceCreatedDomainEventHandler(
    IInvoiceRepository invoiceRepository)
    : IDomainEventHandler<InvoiceCreatedDomainEvent>
{
    public async Task Handle(
        InvoiceCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var invoice = await invoiceRepository.GetByIdAsync(
            notification.Id,
            cancellationToken);

        if (invoice is null)
        {
            return;
        }

        Console.WriteLine($"Invoice created with number : {invoice.InvoiceNumber.Value}");
    }
}

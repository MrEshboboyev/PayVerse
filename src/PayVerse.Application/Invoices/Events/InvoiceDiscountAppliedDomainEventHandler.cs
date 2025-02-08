using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Invoices;
using PayVerse.Domain.Repositories.Invoices;

namespace PayVerse.Application.Invoices.Events;

internal sealed class InvoiceDiscountAppliedDomainEventHandler(
    IInvoiceRepository invoiceRepository)
    : IDomainEventHandler<InvoiceDiscountAppliedDomainEvent>
{
    public async Task Handle(
        InvoiceDiscountAppliedDomainEvent notification,
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

        Console.WriteLine($"Discount of amount : {notification.DiscountAmount} applied" +
                          $" to invoice [ID : {invoice.Id}]");
    }
}
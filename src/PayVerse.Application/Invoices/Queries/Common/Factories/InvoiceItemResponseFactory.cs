using PayVerse.Application.Invoices.Queries.Common.Responses;
using PayVerse.Domain.Entities.Invoices;

namespace PayVerse.Application.Invoices.Queries.Common.Factories;

public static class InvoiceItemResponseFactory
{
    public static InvoiceItemResponse Create(InvoiceItem invoiceItem)
    {
        return new InvoiceItemResponse(
            invoiceItem.Id,
            invoiceItem.Description,
            invoiceItem.Amount.Value);
    }
}
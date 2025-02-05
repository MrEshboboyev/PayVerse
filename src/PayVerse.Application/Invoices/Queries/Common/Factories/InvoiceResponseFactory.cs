using PayVerse.Application.Invoices.Queries.Common.Responses;
using PayVerse.Domain.Entities.Invoices;

namespace PayVerse.Application.Invoices.Queries.Common.Factories;

public static class InvoiceResponseFactory
{
    public static InvoiceResponse Create(Invoice invoice)
    {
        return new InvoiceResponse(
            invoice.Id,
            invoice.InvoiceNumber.Value,
            invoice.InvoiceDate.Value,
            invoice.TotalAmount.Value,
            invoice.UserId);
    }
}
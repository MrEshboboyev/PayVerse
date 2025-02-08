using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Invoices.Commands.AddTaxToInvoice;

public sealed record AddTaxToInvoiceCommand(
    Guid InvoiceId,
    decimal TaxAmount) : ICommand;
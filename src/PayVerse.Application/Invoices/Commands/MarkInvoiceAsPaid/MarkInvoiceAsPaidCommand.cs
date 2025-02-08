using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Invoices.Commands.MarkInvoiceAsPaid;

public sealed record MarkInvoiceAsPaidCommand(
    Guid InvoiceId) : ICommand;
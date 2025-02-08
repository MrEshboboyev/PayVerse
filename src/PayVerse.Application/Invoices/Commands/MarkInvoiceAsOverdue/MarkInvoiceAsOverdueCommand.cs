using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Invoices.Commands.MarkInvoiceAsOverdue;

public sealed record MarkInvoiceAsOverdueCommand(
    Guid InvoiceId) : ICommand;
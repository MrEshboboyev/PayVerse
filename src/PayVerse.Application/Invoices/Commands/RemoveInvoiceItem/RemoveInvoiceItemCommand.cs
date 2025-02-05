using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Invoices.Commands.RemoveInvoiceItem;

public sealed record RemoveInvoiceItemCommand(
    Guid InvoiceId,
    Guid ItemId) : ICommand;
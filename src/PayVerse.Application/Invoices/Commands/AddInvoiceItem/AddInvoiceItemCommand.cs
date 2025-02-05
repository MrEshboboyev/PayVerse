using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Invoices.Commands.AddInvoiceItem;

public sealed record AddInvoiceItemCommand(
    Guid InvoiceId,
    string Description,
    decimal Amount) : ICommand;
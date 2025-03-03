using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Invoices.Commands.CancelInvoice;

public sealed record CancelInvoiceCommand(
    Guid InvoiceId,
    string Reason) : ICommand;

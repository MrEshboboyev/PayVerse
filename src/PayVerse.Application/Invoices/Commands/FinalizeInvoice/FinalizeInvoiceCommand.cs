using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Invoices.Commands.FinalizeInvoice;

public sealed record FinalizeInvoiceCommand(
    Guid InvoiceId) : ICommand;

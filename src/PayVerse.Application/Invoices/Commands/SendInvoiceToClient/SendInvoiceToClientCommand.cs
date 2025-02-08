using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Invoices.Commands.SendInvoiceToClient;

public sealed record SendInvoiceToClientCommand(
    Guid InvoiceId,
    string Email) : ICommand;
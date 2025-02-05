using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Invoices.Commands.CreateInvoice;

public sealed record CreateInvoiceCommand(
    string InvoiceNumber,
    DateTime InvoiceDate,
    decimal TotalAmount,
    Guid UserId) : ICommand;
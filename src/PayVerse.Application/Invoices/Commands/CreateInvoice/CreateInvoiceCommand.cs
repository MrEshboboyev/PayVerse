using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Invoices.Commands.CreateInvoice;

public sealed record CreateInvoiceCommand(
    Guid UserId,
    List<(string Description, decimal Amount)> Items) : ICommand;
using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Invoices.Commands.ApplyDiscountToInvoice;

public sealed record ApplyDiscountToInvoiceCommand(
    Guid InvoiceId,
    decimal DiscountAmount) : ICommand;
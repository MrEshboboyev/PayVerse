namespace PayVerse.Domain.Enums.Invoices;

public enum InvoiceStatus
{
    Draft = 100,
    Sent = 200,
    Paid = 300,
    Overdue = 400,
    Recurring = 500,
    Issued = 600,
    Cancelled = 700,
    Finalized = 800
}
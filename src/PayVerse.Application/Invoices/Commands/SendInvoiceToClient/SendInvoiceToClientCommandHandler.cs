using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Invoices.Commands.SendInvoiceToClient;

internal sealed class SendInvoiceToClientCommandHandler(
    IInvoiceRepository invoiceRepository,
    // INotificationService notificationService,
    IUnitOfWork unitOfWork) : ICommandHandler<SendInvoiceToClientCommand>
{
    public async Task<Result> Handle(
        SendInvoiceToClientCommand request,
        CancellationToken cancellationToken)
    {
        var (invoiceId, email) = request;

        var invoice = await invoiceRepository.GetByIdAsync(invoiceId, cancellationToken);
        if (invoice is null)
        {
            return Result.Failure(DomainErrors.Invoice.NotFound(invoiceId));
        }

        // await notificationService.SendEmailAsync(
        //     email,
        //     "Invoice",
        //     $"Here is your invoice: {invoice.InvoiceNumber}",
        //     cancellationToken);
        
        // TO - DO this functionality -> coming soon

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

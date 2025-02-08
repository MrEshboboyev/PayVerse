using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Invoices.Commands.MarkInvoiceAsPaid;

internal sealed class MarkInvoiceAsPaidCommandHandler(
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<MarkInvoiceAsPaidCommand>
{
    public async Task<Result> Handle(
        MarkInvoiceAsPaidCommand request,
        CancellationToken cancellationToken)
    {
        var invoiceId = request.InvoiceId;

        #region Get this invoice
        
        var invoice = await invoiceRepository.GetByIdAsync(invoiceId, cancellationToken);
        if (invoice is null)
        {
            return Result.Failure(DomainErrors.Invoice.NotFound(invoiceId));
        }
        
        #endregion
        
        #region Mark as paid invoice

        var markAsPaidResult = invoice.MarkAsPaid();
        if (markAsPaidResult.IsFailure)
        {
            return Result.Failure(markAsPaidResult.Error);
        }
        
        #endregion

        await invoiceRepository.UpdateAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Invoices.Commands.MarkInvoiceAsOverdue;

internal sealed class MarkInvoiceAsOverdueCommandHandler(
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<MarkInvoiceAsOverdueCommand>
{
    public async Task<Result> Handle(
        MarkInvoiceAsOverdueCommand request,
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
        
        #region Mark this invoice as overdue

        var markAsOverdueResult = invoice.MarkAsOverdue();
        if (markAsOverdueResult.IsFailure)
        {
            return Result.Failure(markAsOverdueResult.Error);
        }
        
        #endregion

        await invoiceRepository.UpdateAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

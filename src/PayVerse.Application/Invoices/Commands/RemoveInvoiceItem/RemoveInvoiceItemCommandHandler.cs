using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Invoices.Commands.RemoveInvoiceItem;

internal sealed class RemoveInvoiceItemCommandHandler(
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveInvoiceItemCommand>
{
    public async Task<Result> Handle(
        RemoveInvoiceItemCommand request,
        CancellationToken cancellationToken)
    {
        var (invoiceId, invoiceItemId) = request;
        
        #region Get Invoice

        var invoice = await invoiceRepository.GetByIdAsync(invoiceId, cancellationToken);
        if (invoice is null)
        {
            return Result.Failure(
                DomainErrors.Invoice.NotFound(invoiceId));
        }

        #endregion

        #region Remove Invoice Item

        var removeInvoiceItemResult = invoice.RemoveItem(invoiceItemId);
        if (removeInvoiceItemResult.IsFailure)
        {
            return Result.Failure(
                removeInvoiceItemResult.Error);
        }

        #endregion

        #region Save Changes

        await invoiceRepository.UpdateAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}
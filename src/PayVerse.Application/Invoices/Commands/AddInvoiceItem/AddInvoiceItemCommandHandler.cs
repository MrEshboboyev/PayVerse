using InspireEd.Domain.Repositories;
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Invoices;

namespace PayVerse.Application.Invoices.Commands.AddInvoiceItem;

internal sealed class AddInvoiceItemCommandHandler(
    IInvoiceRepository invoiceRepository,
    IInvoiceItemRepository invoiceItemRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddInvoiceItemCommand>
{
    public async Task<Result> Handle(AddInvoiceItemCommand request, CancellationToken cancellationToken)
    {
        var (invoiceId, description, amount) = request;
        
        #region Get Invoice

        var invoice = await invoiceRepository.GetByIdAsync(invoiceId, cancellationToken);
        if (invoice is null)
        {
            return Result.Failure(
                DomainErrors.Invoice.NotFound(request.InvoiceId));
        }

        #endregion
        
        #region Prepare value objects

        var amountResult = Amount.Create(amount);
        if (amountResult.IsFailure)
        {
            return Result.Failure(
                amountResult.Error);
        }
        
        #endregion

        #region Add Invoice Item

        var addInvoiceItemResult = invoice.AddItem(
            description,
            amountResult.Value);
        if (addInvoiceItemResult.IsFailure)
        {
            return Result.Failure(
                addInvoiceItemResult.Error);
        }

        #endregion

        #region Save Changes

        await invoiceRepository.UpdateAsync(invoice, cancellationToken);
        await invoiceItemRepository.AddAsync(addInvoiceItemResult.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}
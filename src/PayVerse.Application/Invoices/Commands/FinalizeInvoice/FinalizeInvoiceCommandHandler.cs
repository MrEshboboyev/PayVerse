using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Shared;
using PayVerse.Domain.Errors;

namespace PayVerse.Application.Invoices.Commands.FinalizeInvoice;

internal sealed class FinalizeInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<FinalizeInvoiceCommand>
{
    public async Task<Result> Handle(
        FinalizeInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var invoiceId = request.InvoiceId;

        var invoice = await invoiceRepository.GetByIdAsync(invoiceId, cancellationToken);

        if (invoice is null)
        {
            return Result.Failure(
                DomainErrors.Invoice.NotFound(invoiceId));
        }

        var result = invoice.Finalize();

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await invoiceRepository.UpdateAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

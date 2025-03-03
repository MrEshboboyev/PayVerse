using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Shared;
using PayVerse.Domain.Errors;

namespace PayVerse.Application.Invoices.Commands.CancelInvoice;

internal sealed class CancelInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CancelInvoiceCommand>
{
    public async Task<Result> Handle(
        CancelInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var (invoiceId, reason) = request;

        var invoice = await invoiceRepository.GetByIdAsync(invoiceId, cancellationToken);

        if (invoice is null)
        {
            return Result.Failure(
                DomainErrors.Invoice.NotFound(invoiceId));
        }

        var result = invoice.Cancel(reason);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await invoiceRepository.UpdateAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

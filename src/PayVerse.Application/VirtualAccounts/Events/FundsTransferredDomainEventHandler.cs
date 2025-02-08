using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.VirtualAccounts;
using PayVerse.Domain.Repositories.VirtualAccounts;

namespace PayVerse.Application.VirtualAccounts.Events;

internal sealed class FundsTransferredDomainEventHandler(
    IVirtualAccountRepository virtualAccountRepository) : IDomainEventHandler<FundsTransferredDomainEvent>
{
    public async Task Handle(
        FundsTransferredDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var fromAccount = await virtualAccountRepository.GetByIdAsync(notification.FromAccountId, cancellationToken);
        var toAccount = await virtualAccountRepository.GetByIdAsync(notification.ToAccountId, cancellationToken);

        if (fromAccount is null || toAccount is null)
        {
            return;
        }

        Console.WriteLine($"Transferred {notification.Amount}" +
                          $" from account [ID: {fromAccount.Id}]" +
                          $" to account [ID: {toAccount.Id}].");
    }
}
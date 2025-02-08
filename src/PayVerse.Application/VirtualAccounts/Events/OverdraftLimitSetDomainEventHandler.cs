using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.VirtualAccounts;
using PayVerse.Domain.Repositories.VirtualAccounts;

namespace PayVerse.Application.VirtualAccounts.Events;

internal sealed class OverdraftLimitSetDomainEventHandler(
    IVirtualAccountRepository virtualAccountRepository) : IDomainEventHandler<OverdraftLimitSetDomainEvent>
{
    public async Task Handle(
        OverdraftLimitSetDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var account = await virtualAccountRepository.GetByIdAsync(notification.AccountId, cancellationToken);
        if (account is null)
        {
            return;
        }

        Console.WriteLine($"Overdraft limit of {notification.OverdraftLimit} set" +
                          $" for account [ID: {account.Id}].");
    }
}

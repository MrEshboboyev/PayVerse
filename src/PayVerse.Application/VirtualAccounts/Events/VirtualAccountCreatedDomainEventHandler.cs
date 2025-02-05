using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.VirtualAccounts;
using PayVerse.Domain.Repositories.VirtualAccounts;

namespace PayVerse.Application.VirtualAccounts.Events;

internal sealed class VirtualAccountCreatedDomainEventHandler(
    IVirtualAccountRepository virtualAccountRepository) : IDomainEventHandler<VirtualAccountCreatedDomainEvent>
{
    public async Task Handle(
        VirtualAccountCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var virtualAccount = await virtualAccountRepository.GetByIdAsync(
            notification.Id,
            cancellationToken);

        if (virtualAccount is null)
        {
            return;
        }

        Console.WriteLine($"Created virtual account with number: {virtualAccount.AccountNumber}");
    }
}
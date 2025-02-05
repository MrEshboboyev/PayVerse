using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Users;
using PayVerse.Domain.Repositories.Users;

namespace PayVerse.Application.Users.Events;

internal sealed class UserRegisteredDomainEventHandler(
    IUserRepository userRepository)
          : IDomainEventHandler<UserCreatedDomainEvent>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task Handle(
        UserCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            notification.UserId,
            cancellationToken);

        if (user is null)
        {
            return;
        }

        Console.WriteLine($"User created with email : {user.Email.Value}");
    }
}

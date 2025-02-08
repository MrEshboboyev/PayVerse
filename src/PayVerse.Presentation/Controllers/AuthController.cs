using MediatR;
using Microsoft.AspNetCore.Mvc;
using PayVerse.Application.Users.Commands.CreateUser;
using PayVerse.Application.Users.Commands.Login;
using PayVerse.Domain.Entities.Users;
using PayVerse.Presentation.Abstractions;
using PayVerse.Presentation.Contracts.Users;

namespace PayVerse.Presentation.Controllers;

[Route("api/auth")]
public sealed class AuthController(ISender sender) : ApiController(sender) 
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        #region Get Role from Name

        var role = Role.FromName(request.RoleName);
        if (!new[] { Role.IndividualUser, Role.BusinessUser }.Contains(role))
        {
            return BadRequest();
        }

        #endregion
        
        var command = new CreateUserCommand(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            role.Id);

        var result = await Sender.Send(command, cancellationToken);
        
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }
}
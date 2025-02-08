using PayVerse.Application.Users.Commands.CreateUser;
using PayVerse.Application.Users.Commands.Login;
using PayVerse.Application.Users.Queries.GetUserById;
using PayVerse.Presentation.Abstractions;
using PayVerse.Presentation.Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayVerse.Application.Users.Commands.BlockUser;
using PayVerse.Application.Users.Commands.ChangePassword;
using PayVerse.Application.Users.Commands.EnableTwoFactorAuthentication;
using PayVerse.Application.Users.Commands.UnblockUser;
using PayVerse.Application.Users.Commands.UpdateUser;
using PayVerse.Domain.Entities.Users;

namespace PayVerse.Presentation.Controllers;

[Route("api/users")]
public sealed class UsersController(ISender sender) : ApiController(sender)
{
    #region Get

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCurrentUserById(CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(GetUserId());

        var response = await Sender.Send(query, cancellationToken);
        
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    #endregion

    #region Auth related

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

    #endregion
    
    #region Post endpoints 

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser(
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(
            GetUserId(),
            request.FirstName,
            request.LastName);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangePasswordCommand(
            request.Email,
            request.OldPassword,
            request.NewPassword);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("enable-2fa")]
    public async Task<IActionResult> EnableTwoFactorAuthentication(
        CancellationToken cancellationToken)
    {
        var command = new EnableTwoFactorAuthenticationCommand(GetUserId());
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }
    
    #endregion
}
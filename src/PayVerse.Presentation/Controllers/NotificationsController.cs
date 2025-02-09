using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayVerse.Application.Notifications.Commands.MarkNotificationAsRead;
using PayVerse.Application.Notifications.Queries.GetNotifications;
using PayVerse.Presentation.Abstractions;

namespace PayVerse.Presentation.Controllers;

[Route("api/notifications")]
[Authorize]
public sealed class NotificationsController(ISender sender) : ApiController(sender)
{
    #region Get endpoints
    
    [HttpGet("user")]
    public async Task<IActionResult> GetNotifications(
        CancellationToken cancellationToken)
    {
        var query = new GetNotificationsQuery(GetUserId());
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    #endregion
    
    #region Put endpoints
    
    [HttpPut("{notificationId:guid}")]
    public async Task<IActionResult> MarkAsRead(
        Guid notificationId,
        CancellationToken cancellationToken)
    {
        var query = new MarkNotificationAsReadCommand(notificationId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsFailure ? HandleFailure(response) : Ok(response);
    }
    
    #endregion
}
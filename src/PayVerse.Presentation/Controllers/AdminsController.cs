using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayVerse.Application.Invoices.Queries.GetAllInvoices;
using PayVerse.Application.Invoices.Queries.GetInvoicesByUserId;
using PayVerse.Application.VirtualAccounts.Queries.GetAllVirtualAccounts;
using PayVerse.Application.VirtualAccounts.Queries.GetVirtualAccountsByUserId;
using PayVerse.Presentation.Abstractions;

namespace PayVerse.Presentation.Controllers;

[Route("api/admins")]
[Authorize(Roles = "Admin")]
public sealed class AdminsController(ISender sender) : ApiController(sender)
{
    #region Get Endpoints

    #region Invoices
    
    [HttpGet]
    public async Task<IActionResult> GetAllInvoices(CancellationToken cancellationToken)
    {
        var query = new GetAllInvoicesQuery();
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetInvoicesByUserId(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetInvoicesByUserIdQuery(userId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    #endregion
    
    #region VirtualAccounts
    
    [HttpGet]
    public async Task<IActionResult> GetAllVirtualAccounts(CancellationToken cancellationToken)
    {
        var query = new GetAllVirtualAccountsQuery();
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetVirtualAccountsByUserId(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetVirtualAccountsByUserIdQuery(userId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    #endregion
    
    #endregion
    
    
}
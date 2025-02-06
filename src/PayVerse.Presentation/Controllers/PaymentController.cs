using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayVerse.Application.Payments.Commands.CreatePayment;
using PayVerse.Application.Payments.Commands.UpdatePaymentStatus;
using PayVerse.Application.Payments.Queries.GetPaymentById;
using PayVerse.Application.Payments.Queries.GetPaymentsByStatus;
using PayVerse.Application.Payments.Queries.GetPaymentsByUserId;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Presentation.Abstractions;
using PayVerse.Presentation.Contracts.Payments;

namespace PayVerse.Presentation.Controllers;

[Route("api/payments")]
[Authorize]
public sealed class PaymentController(ISender sender) : ApiController(sender)
{
    #region Get Endpoints

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPaymentById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentByIdQuery(id);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetPayments(
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentsByUserIdQuery(GetUserId());
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetPaymentsByStatus(
        PaymentStatus status,
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentsByStatusQuery(status);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    #endregion

    #region Post Endpoints

    [HttpPost]
    public async Task<IActionResult> CreatePayment(
        [FromBody]
        CreatePaymentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreatePaymentCommand(
            request.Amount,
            request.Status,
            GetUserId());
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    #endregion

    #region Patch Endpoints

    [HttpPatch("{id:guid}/status/{status}")]
    public async Task<IActionResult> UpdatePaymentStatus(
        Guid id,
        PaymentStatus status,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePaymentStatusCommand(id, status);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    #endregion
}

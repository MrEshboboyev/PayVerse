using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayVerse.Application.Invoices.Commands.AddInvoiceItem;
using PayVerse.Application.Invoices.Commands.AddTaxToInvoice;
using PayVerse.Application.Invoices.Commands.ApplyDiscountToInvoice;
using PayVerse.Application.Invoices.Commands.CreateInvoice;
using PayVerse.Application.Invoices.Commands.CreateRecurringInvoice;
using PayVerse.Application.Invoices.Commands.MarkInvoiceAsOverdue;
using PayVerse.Application.Invoices.Commands.MarkInvoiceAsPaid;
using PayVerse.Application.Invoices.Commands.RemoveInvoiceItem;
using PayVerse.Application.Invoices.Commands.SendInvoiceToClient;
using PayVerse.Application.Invoices.Queries.GetInvoiceById;
using PayVerse.Application.Invoices.Queries.GetInvoiceItemById;
using PayVerse.Application.Invoices.Queries.GetInvoiceItems;
using PayVerse.Application.Invoices.Queries.GetInvoicesByUserId;
using PayVerse.Application.Invoices.Queries.GetInvoiceWithItemsById;
using PayVerse.Application.Invoices.Queries.GetTotalRevenueByUser;
using PayVerse.Presentation.Abstractions;
using PayVerse.Presentation.Contracts.Invoices;

namespace PayVerse.Presentation.Controllers;

[Route("api/invoices")]
[Authorize]
public sealed class InvoiceController(ISender sender) : ApiController(sender)
{
    #region Get Endpoints

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetInvoiceById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetInvoiceByIdQuery(id);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetInvoices(
        CancellationToken cancellationToken)
    {
        var query = new GetInvoicesByUserIdQuery(GetUserId());
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("{invoiceId:guid}/items")]
    public async Task<IActionResult> GetInvoiceItems(
        Guid invoiceId,
        CancellationToken cancellationToken)
    {
        var query = new GetInvoiceItemsQuery(invoiceId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("{invoiceId:guid}/items/{itemId:guid}")]
    public async Task<IActionResult> GetInvoiceItemById(
        Guid invoiceId,
        Guid itemId,
        CancellationToken cancellationToken)
    {
        var query = new GetInvoiceItemByIdQuery(invoiceId, itemId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("{id:guid}/with-items")]
    public async Task<IActionResult> GetInvoiceWithItemsById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetInvoiceWithItemsByIdQuery(id);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("total-revenue")]
    public async Task<IActionResult> GetTotalRevenue(
        CancellationToken cancellationToken)
    {
        var query = new GetTotalRevenueByUserQuery(GetUserId());
        var result = await Sender.Send(query, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    #endregion

    #region Post Endpoints

    [HttpPost("create-invoice")]
    public async Task<IActionResult> CreateInvoice(
        [FromBody] CreateInvoiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateInvoiceCommand(
            GetUserId(),
            [.. request.Items.Select(item => (item.Description, item.Amount))]);

        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("{invoiceId:guid}/items")]
    public async Task<IActionResult> AddInvoiceItem(
        Guid invoiceId,
        [FromBody] AddInvoiceItemRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddInvoiceItemCommand(
            invoiceId,
            request.Description,
            request.Amount);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("send-to-client")]
    public async Task<IActionResult> SendInvoiceToClient(
        [FromBody] SendInvoiceToClientRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SendInvoiceToClientCommand(
            request.InvoiceId,
            request.Email);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("{invoiceId:guid}/mark-as-paid")]
    public async Task<IActionResult> MarkInvoiceAsPaid(
        Guid invoiceId,
        CancellationToken cancellationToken)
    {
        var command = new MarkInvoiceAsPaidCommand(invoiceId);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("{invoiceId:guid}/mark-as-overdue")]
    public async Task<IActionResult> MarkInvoiceAsOverdue(
        Guid invoiceId,
        CancellationToken cancellationToken)
    {
        var command = new MarkInvoiceAsOverdueCommand(invoiceId);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("apply-discount")]
    public async Task<IActionResult> ApplyDiscountToInvoice(
        [FromBody] ApplyDiscountToInvoiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ApplyDiscountToInvoiceCommand(
            request.InvoiceId,
            request.DiscountAmount);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("add-tax")]
    public async Task<IActionResult> AddTaxToInvoice(
        [FromBody] AddTaxToInvoiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddTaxToInvoiceCommand(
            request.InvoiceId,
            request.TaxAmount);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("create-recurring")]
    public async Task<IActionResult> CreateRecurringInvoice(
    [FromBody] CreateRecurringInvoiceRequest request,
    CancellationToken cancellationToken)
    {
        var command = new CreateRecurringInvoiceCommand(
            GetUserId(),
            request.FrequencyInMonths,
            request.Items);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    #endregion

    #region Delete Endpoints

    [HttpDelete("{invoiceId:guid}/items/{itemId:guid}")]
    public async Task<IActionResult> RemoveInvoiceItem(
        Guid invoiceId,
        Guid itemId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveInvoiceItemCommand(invoiceId, itemId);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    #endregion
}
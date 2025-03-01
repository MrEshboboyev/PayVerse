using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayVerse.Application.Reports.Commands.GenerateCompositeFinancialReport;
using PayVerse.Application.Reports.Queries.GetCompositeFinancialReportById;
using PayVerse.Application.Reports.Queries.GetCompositeFinancialReportsByUser;
using PayVerse.Application.Reports.Queries.GetCompositeReportById;
using PayVerse.Presentation.Abstractions;
using PayVerse.Presentation.Contracts.Reports;

namespace PayVerse.Presentation.Controllers;

[Route("api/reports")]
[Authorize]
public sealed class ReportController(ISender sender) : ApiController(sender)
{
    [HttpGet("{reportId:guid}")]
    public async Task<IActionResult> GetReportById(
        Guid reportId,
        CancellationToken cancellationToken)
    {
        var query = new GetCompositeFinancialReportByIdQuery(reportId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    [HttpGet("{reportId:guid}/file")]
    public async Task<IActionResult> GetReportFile(
        Guid reportId,
        CancellationToken cancellationToken)
    {
        // Fetch report details
        var query = new GetCompositeFinancialReportByIdQuery(reportId);
        var response = await Sender.Send(query, cancellationToken);

        if (!response.IsSuccess)
        {
            return NotFound(response.Error);
        }

        // Get the file path from the report details
        var filePath = response.Value.FilePath;

        // Ensure the file exists
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("Report file not found.");
        }

        // Read the file content
        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath, cancellationToken);
        var contentType = GetContentType(filePath);
        var fileName = Path.GetFileName(filePath);

        return File(fileBytes, contentType, fileName);
    }

    private string GetContentType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension switch
        {
            ".pdf" => "application/pdf",
            ".csv" => "text/csv",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".html" => "text/html",
            ".json" => "application/json",
            ".txt" => "text/plain",
            _ => "application/octet-stream",
        };
    }

    [HttpGet]
    public async Task<IActionResult> GetUserReports(CancellationToken cancellationToken)
    {
        var query = new GetCompositeFinancialReportsByUserQuery(GetUserId());
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpPost("composite")]
    public async Task<IActionResult> GenerateCompositeReport(
        [FromBody] GenerateReportRequest request,
        CancellationToken cancellationToken)
    {
        var command = new GenerateCompositeFinancialReportCommand(
            GetUserId(),
            request.Title,
            request.StartDate,
            request.EndDate,
            request.Type,
            request.FileType);

        var reportIdResponse = await Sender.Send(command, cancellationToken);

        return reportIdResponse.IsSuccess ? Ok(reportIdResponse.Value) : BadRequest(reportIdResponse.Error);
    }

    [HttpGet("composite/{id}")]
    public async Task<IActionResult> GetCompositeReport(Guid id)
    {
        var result = await Sender.Send(new GetCompositeReportByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result.Error);

        return Ok(result.Value);
    }
}
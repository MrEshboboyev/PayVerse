using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayVerse.Application.Reports.Commands.GenerateFinancialReport;
using PayVerse.Application.Reports.Queries.GetFinancialReportById;
using PayVerse.Application.Reports.Queries.GetFinancialReportsByUser;
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
        var query = new GetFinancialReportByIdQuery(reportId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    [HttpGet("{reportId:guid}/file")]
    public async Task<IActionResult> GetReportFile(
        Guid reportId,
        CancellationToken cancellationToken)
    {
        // Fetch report details
        var query = new GetFinancialReportByIdQuery(reportId);
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
        var query = new GetFinancialReportsByUserQuery(GetUserId());
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpPost]
    public async Task<IActionResult> GenerateReport(
        [FromBody] GenerateReportRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new GenerateFinancialReportCommand(
            GetUserId(),
            request.StartDate,
            request.EndDate,
            request.Type,
            request.FileType);
        var response = await Sender.Send(command, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
    }
}
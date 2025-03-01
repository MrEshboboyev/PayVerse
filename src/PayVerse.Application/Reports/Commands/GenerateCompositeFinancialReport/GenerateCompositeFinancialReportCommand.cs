using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Enums.Reports;

namespace PayVerse.Application.Reports.Commands.GenerateCompositeFinancialReport;

/// <summary>
/// Command for generating a financial report using the Composite pattern
/// </summary>
public record GenerateCompositeFinancialReportCommand(
    Guid UserId,
    string Title,
    DateOnly StartDate,
    DateOnly EndDate,
    ReportType Type,
    FileType FileType) : ICommand<Guid>;

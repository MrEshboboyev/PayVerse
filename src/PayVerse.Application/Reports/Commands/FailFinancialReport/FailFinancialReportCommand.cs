using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Reports.Commands.FailFinancialReport;

public sealed record FailFinancialReportCommand(
    Guid ReportId) : ICommand;
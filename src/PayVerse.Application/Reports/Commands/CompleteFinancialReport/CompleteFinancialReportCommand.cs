using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Reports.Commands.CompleteFinancialReport;

public sealed record CompleteFinancialReportCommand(
    Guid ReportId,
    string FilePath) : ICommand;
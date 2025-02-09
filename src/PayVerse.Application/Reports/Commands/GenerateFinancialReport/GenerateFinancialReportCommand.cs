using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Enums.Reports;

namespace PayVerse.Application.Reports.Commands.GenerateFinancialReport;

public sealed record GenerateFinancialReportCommand(
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate,
    ReportType Type) : ICommand<Guid>;
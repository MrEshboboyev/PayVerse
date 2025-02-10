using Newtonsoft.Json;
using PayVerse.Application.Reports.Services;
using PayVerse.Domain.Entities.Reports;

namespace PayVerse.Infrastructure.ReportGenerators;

internal sealed class JsonReportGenerator : IReportGenerator
{
    public async Task<string> GenerateAsync(FinancialReport report, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine("GeneratedReports", $"{report.Id}.json");

        var jsonContent = JsonConvert.SerializeObject(new
        {
            report.Type,
            report.Period,
            report.GeneratedBy,
            report.Status,
            report.GeneratedAt
        });

        await File.WriteAllTextAsync(filePath, jsonContent, cancellationToken);

        return filePath;
    }
}

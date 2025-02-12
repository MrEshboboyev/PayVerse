using Newtonsoft.Json;
using PayVerse.Application.Reports.Services;
using PayVerse.Domain.Entities.Reports;

namespace PayVerse.Infrastructure.Reports.Generators;

public class JsonReportGenerator : IReportGenerator
{
    public async Task<string> GenerateAsync(FinancialReport report, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine("GeneratedReports", $"{report.Id}.json");

        var jsonContent = JsonConvert.SerializeObject(new
        {
            report.Id,
            report.Period.StartDate,
            report.Period.EndDate,
            report.Type,
            report.GeneratedBy,
            report.Status,
            report.GeneratedAt
        }, Formatting.Indented);

        await File.WriteAllTextAsync(filePath, jsonContent, cancellationToken);

        return filePath;
    }
}
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Persistence.Reports.Configurations;

public class ReportPeriodConverter() : ValueConverter<ReportPeriod, string>(period => Serialize(period),
    value => Deserialize(value))
{
    private static string Serialize(ReportPeriod period)
    {
        return $"{period.StartDate:yyyy-MM-dd},{period.EndDate:yyyy-MM-dd}";
    }

    private static ReportPeriod Deserialize(string value)
    {
        var parts = value.Split(',');
        return ReportPeriod.Create(
                DateOnly.Parse(parts[0]),
                DateOnly.Parse(parts[1]))
            .Value;
    }
}

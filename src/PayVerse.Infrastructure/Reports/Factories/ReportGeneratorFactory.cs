using Microsoft.Extensions.DependencyInjection;
using PayVerse.Application.Reports.Services;
using PayVerse.Domain.Enums.Reports;
using PayVerse.Infrastructure.Reports.Generators;

namespace PayVerse.Infrastructure.Reports.Factories;

public sealed class ReportGeneratorFactory(IServiceProvider serviceProvider) : IReportGeneratorFactory
{
    public IReportGenerator CreateReportGenerator(FileType fileType)
    {
        return fileType switch
        {
            FileType.Pdf => serviceProvider.GetRequiredService<PdfReportGenerator>(),
            FileType.Txt => serviceProvider.GetRequiredService<TxtReportGenerator>(),
            FileType.Html => serviceProvider.GetRequiredService<HtmlReportGenerator>(),
            FileType.Csv => serviceProvider.GetRequiredService<CsvReportGenerator>(),
            FileType.Json => serviceProvider.GetRequiredService<JsonReportGenerator>(),
            FileType.Excel => serviceProvider.GetRequiredService<ExcelReportGenerator>(),
            _ => throw new ArgumentException(
                "Invalid/Unsupported file type", 
                nameof(fileType))
        };
    }
}

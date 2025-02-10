using Microsoft.Extensions.DependencyInjection;
using PayVerse.Application.Reports.Services;
using PayVerse.Domain.Enums.Reports;

namespace PayVerse.Infrastructure.ReportGenerators;

public sealed class ReportGeneratorFactory(IServiceProvider serviceProvider)
{
    public IReportGenerator GetGenerator(FileType fileType)
    {
        return fileType switch
        {
            FileType.Pdf => serviceProvider.GetRequiredService<PdfReportGenerator>(),
            FileType.Txt => serviceProvider.GetRequiredService<TxtReportGenerator>(),
            FileType.Html => serviceProvider.GetRequiredService<HtmlReportGenerator>(),
            FileType.Csv => serviceProvider.GetRequiredService<CsvReportGenerator>(),
            FileType.Json => serviceProvider.GetRequiredService<JsonReportGenerator>(),
            FileType.Excel => serviceProvider.GetRequiredService<ExcelReportGenerator>(),
            _ => throw new NotImplementedException($"File type {fileType} is not supported.")
        };
    }
}

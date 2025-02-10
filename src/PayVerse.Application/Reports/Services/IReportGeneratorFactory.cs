using PayVerse.Domain.Enums.Reports;

namespace PayVerse.Application.Reports.Services;

public interface IReportGeneratorFactory
{
    IReportGenerator CreateReportGenerator(FileType fileType);
}
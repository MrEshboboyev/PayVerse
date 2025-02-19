namespace PayVerse.Domain.Reports;

public class Report(string content, string type)
{
    public string Content { get; } = content;
    public string Type { get; } = type;
}
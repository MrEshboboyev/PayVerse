using PayVerse.Domain.Errors;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.ValueObjects.Reports;

public sealed class ReportTitle : ValueObject
{
    #region Constants
    
    public const int MaxLength = 50; // Maximum length for an ReportTitle

    #endregion

    #region Constructors

    private ReportTitle(string value)
    {
        Value = value;
    }
    
    #endregion
    
    #region Properties
    
    public string Value { get; }
    
    #endregion

    #region Factory Methods

    /// <summary> 
    /// Creates a ReportTitle instance after validating the input. 
    /// </summary> 
    /// <param name="reportTitle">The first name string to create the ReportTitle value object from.</param> 
    /// <returns>A Result object containing the ReportTitle value object or an error.</returns>
    public static Result<ReportTitle> Create(string reportTitle)
    {
        if (string.IsNullOrWhiteSpace(reportTitle))
        {
            return Result.Failure<ReportTitle>(DomainErrors.ReportTitle.Empty);
        }
        
        if (reportTitle.Length > MaxLength)
        {
            return Result.Failure<ReportTitle>(DomainErrors.ReportTitle.TooLong);
        }
        
        return Result.Success(new ReportTitle(reportTitle));
    }
    
    #endregion

    #region Overrides
    
    /// <summary> 
    /// Returns the atomic values of the ReportTitle object for equality checks. 
    /// </summary>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}
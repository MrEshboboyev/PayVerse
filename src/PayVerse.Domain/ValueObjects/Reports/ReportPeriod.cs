using PayVerse.Domain.Errors;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.ValueObjects.Reports;

public sealed class ReportPeriod : ValueObject
{
    #region Properties
    
    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }
    
    #endregion
    
    #region Constructors

    private ReportPeriod(DateOnly startDate, DateOnly endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }
    
    #endregion
    
    #region Factory Methods

    public static Result<ReportPeriod> Create(DateOnly start, DateOnly end)
    {
        if (start > end)
        {
            return Result.Failure<ReportPeriod>(
                DomainErrors.ReportPeriod.InvalidDateRange(start, end));
        }

        return Result.Success(new ReportPeriod(start, end));
    }
    
    #endregion
    
    #region Overrides

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return StartDate;
        yield return EndDate;
    }
    
    #endregion
}
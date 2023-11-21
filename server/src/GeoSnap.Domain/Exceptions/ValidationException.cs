using FluentValidation.Results;

namespace GeoSnap.Domain.Exceptions;
public class ValidationException : FluentValidation.ValidationException
{
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base(failures)
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}

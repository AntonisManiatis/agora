using ErrorOr;

namespace Agora.API;

using static Results;

static class ErrorOrExtensions
{
    internal static IResult ToProblem(this List<Error> errors)
    {
        if (errors is null or [])
        {
            return Problem();
        }

        if (errors.All(err => err.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors.ToDictionary(
                err => err.Code,
                err => new[] { err.Description }
            ));
        }

        var err = errors[0];

        var statusCode = err.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: err.Description);
    }

    // ? Name?
    internal static IResult MatchOk<T>(this ErrorOr<T> errorOr, Func<T, IResult> func)
    {
        return errorOr.Match<IResult>(
            func,
            errors => errors.ToProblem()
        );
    }
}
using ErrorOr;

namespace Agora.API;

static class ErrorOrExtensions
{
    // TODO: Test this
    internal static IResult ToProblem(this List<Error> errors)
    {
        // ? Empty??
        if (errors is null)
        {
            return Results.Empty;
        }

        var err = errors[0];

        var statusCode = err.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.Problem(statusCode: statusCode, title: err.Description);
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
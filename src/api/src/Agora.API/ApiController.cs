using ErrorOr;

using Microsoft.AspNetCore.Mvc;

namespace Agora.API;

[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(IList<Error> errors)
    {
        HttpContext.Items["errors"] = errors;

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
}

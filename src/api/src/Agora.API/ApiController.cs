using ErrorOr;

using Microsoft.AspNetCore.Mvc;

namespace Agora.API;

[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(IList<Error> errors)
    {
        // TODO: Temp.
        return Problem();
    }
}

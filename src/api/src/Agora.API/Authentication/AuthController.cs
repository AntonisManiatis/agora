using Agora.Identity.Core;
using Agora.Identity.Services;

using Mapster;

using Microsoft.AspNetCore.Mvc;

namespace Agora.API;

public record LoginRequest(string Email, string Password);

[Route("[controller]")]
public class AuthController : ApiController
{
    private readonly IAuthenticationService authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync(LoginRequest req)
    {
        var result = await authenticationService.AuthenticateAsync(req.Adapt<AuthenticationCommand>());

        if (result.IsError && result.FirstError == Errors.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: result.FirstError.Description
            );
        }

        return result.Match<IActionResult>(
            token => Ok(token),
            errors => Problem(errors)
        );
    }
}
using Agora.Identity.Core;
using Agora.Identity.Services;

using Mapster;

using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Identity.Tokens;

public record LoginRequest(string Email, string Password);

[Route("[controller]")]
public class TokensController : ApiController
{
    private readonly ITokenService tokenService;

    public TokensController(ITokenService tokenService)
    {
        this.tokenService = tokenService;
    }

    [HttpPost]
    public async Task<IActionResult> IssueAsync(LoginRequest req)
    {
        var result = await tokenService.IssueAsync(req.Adapt<IssueTokenCommand>());

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
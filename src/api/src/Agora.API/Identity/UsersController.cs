using Agora.Identity.Services;

using Mapster;

using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Identity;

public record RegistrationRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password
);

[Route("[controller]")]
public class UsersController : ApiController
{
    private readonly IUserService userService;

    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegistrationRequest request)
    {
        var result = await userService.RegisterUserAsync(request.Adapt<RegisterCommand>());

        // TODO: don't return this yet!
        return result.Match(
            userId => Ok(),
            errors => Problem(errors)
        );
    }
}
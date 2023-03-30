using Agora.Identity.Services;

using Mapster;

using Microsoft.AspNetCore.Mvc;

namespace Agora.API;

public record RegistrationRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password
);

[Route("[controller]")]
public class UsersController : ApiController
{
    private readonly UserService userService;

    public UsersController(UserService userService)
    {
        this.userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegistrationRequest request)
    {
        var result = await userService.RegisterUserAsync(request.Adapt<RegisterCommand>());

        throw new NotImplementedException();
    }
}
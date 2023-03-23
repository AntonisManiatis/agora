using Microsoft.AspNetCore.Mvc;

namespace Agora.API;

public record LoginRequest(string Email, string Password);

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpPost]
    [Route("login")]
    public ActionResult Login([FromBody] LoginRequest req)
    {
        // TODO: Implement later
        return Ok();
    }
}
using Microsoft.AspNetCore.Mvc;

namespace Agora.API;

public record LoginRequest(string Email, string Password);

[Route("[controller]")]
public class AuthController : ApiController
{
    [HttpPost]
    [Route("login")]
    public ActionResult Login(LoginRequest req)
    {
        // TODO: Implement later
        return Ok();
    }
}
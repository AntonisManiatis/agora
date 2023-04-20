using System.Security.Claims;

namespace Agora.API;

static class ClaimsPrincipalExtensions
{
    internal static Guid GetUserId(this ClaimsPrincipal principal)
    {
        // Related: https://stackoverflow.com/questions/46112258/how-do-i-get-current-user-in-net-core-web-api-from-jwt-token
        var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (Guid.TryParse(id, out Guid uid))
        {
            return uid;
        }

        return Guid.Empty;
    }
}
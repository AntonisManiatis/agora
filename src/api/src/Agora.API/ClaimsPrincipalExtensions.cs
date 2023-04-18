using System.Security.Claims;

namespace Agora.API;

internal static class ClaimsPrincipalExtension
{
    internal static Guid GetUserId(this ClaimsPrincipal principal)
    {
        // Related: https://stackoverflow.com/questions/46112258/how-do-i-get-current-user-in-net-core-web-api-from-jwt-token
        var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid uid;

        if (Guid.TryParse(id, out uid))
        {
            return uid;
        }
        else
        {
            return Guid.Empty;
        }
    }
}
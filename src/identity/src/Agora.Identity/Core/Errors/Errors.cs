using ErrorOr;

namespace Agora.Identity.Core;

public static class Errors
{
    public static readonly Error InvalidCredentials =
        Error.Conflict(code: "InvalidCredentials", description: "Invalid credentials.");
}
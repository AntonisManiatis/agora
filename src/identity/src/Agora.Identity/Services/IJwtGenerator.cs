using Agora.Identity.Core;

namespace Agora.Identity.Services; // ? Should this be in this namespace?

internal interface IJwtGenerator
{
    // ? Maybe should return ErrorOr. signing can fail.
    string GenerateToken(User user);
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Agora.Identity.Core;
using Agora.Identity.Services;

using Microsoft.IdentityModel.Tokens;

namespace Agora.Identity.Infrastructure.Tokens;

/// <summary>
/// Uses a symmetric key to sign credentials.
/// </summary>
internal sealed class JwtGenerator : IJwtGenerator
{
    private readonly Func<JwtOptions> optionsFunc;

    public JwtGenerator(Func<JwtOptions> optionsFunc)
    {
        this.optionsFunc = optionsFunc;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            // TODO: add correct claims. Sub vs UniqueName?
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };

        // TODO: Because the same options are used in the ASP.NET web api, it'd be a good idea to 
        // keep them in a common place.

        var options = optionsFunc();
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret)),
            SecurityAlgorithms.HmacSha256
        );

        var securityToken = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims, // TODO: add.
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
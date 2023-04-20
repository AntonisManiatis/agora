using Agora.Identity.Infrastructure.Tokens;
using Agora.Identity.Services;
using Agora.Shared;
using Agora.Shared.Infrastructure;
using Agora.Shared.IntegrationTests;

using Microsoft.Extensions.DependencyInjection;

namespace Agora.Identity.IntegrationTests;

public sealed class ServiceFixture : IDisposable
{
    private readonly ServiceProvider? provider;

    public ServiceFixture(PostgreSqlFixture fixture)
    {
        var services = new ServiceCollection();
        services.AddShared(fixture.ConnectionString!);
        services.AddMigrations(fixture.ConnectionString!, typeof(IUserService).Assembly);

        services.AddIdentity(_ => () => new JwtOptions
        {
            Issuer = "Agora",
            Audience = "Agora",
            Expires = TimeSpan.FromMinutes(5),
            Secret = "super-secret-key"
        });

        provider = services.BuildServiceProvider(validateScopes: true);

        provider.MigrateUp();
    }

    internal Scoped<ITokenService> TokenService => new(provider!.CreateScope());

    internal Scoped<IUserService> UserService => new(provider!.CreateScope());

    public void Dispose() => provider?.Dispose();
}
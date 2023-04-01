using Agora.Identity.Infrastructure;
using Agora.Identity.Infrastructure.Data;
using Agora.Identity.Infrastructure.Tokens;
using Agora.Identity.Services;
using Agora.Shared.Infrastructure;
using Agora.Shared.Infrastructure.Data;
using Agora.Shared.Infrastructure.Messaging;

using Dapper;

using Microsoft.Extensions.DependencyInjection;

using Testcontainers.PostgreSql;

namespace Agora.Identity.IntegrationTests;

public class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer dbContainer =
        new PostgreSqlBuilder().Build();

    private ServiceProvider? provider;
    private IServiceScope? fixtureScope;

    internal ITokenService TokenService =>
        fixtureScope!.ServiceProvider!.GetRequiredService<ITokenService>();
    internal IUserService UserService => fixtureScope!.ServiceProvider!.GetRequiredService<IUserService>();
    internal IDbConnector Connector => provider!.GetRequiredService<IDbConnector>();

    public async Task InitializeAsync()
    {
        await dbContainer.StartAsync();

        var cs = dbContainer.GetConnectionString();

        var services = new ServiceCollection();
        services.AddPostgreSql(cs);
        services.TryAddMessaging();
        services.AddMigrations(cs, typeof(IUserService).Assembly);

        services.AddIdentity(sp => () => new JwtOptions
        {
            Issuer = "Agora",
            Audience = "Agora",
            Expires = TimeSpan.FromMinutes(5),
            Secret = "super-secret-key"
        });

        provider = services.BuildServiceProvider(validateScopes: true);
        fixtureScope = provider.CreateScope();

        var connector = provider.GetRequiredService<IDbConnector>();
        using var connection = await connector.ConnectAsync();

        await connection.ExecuteAsync(@"
            CREATE DATABASE agora;
            CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";
        ");

        provider.MigrateUp();
    }

    public async Task DisposeAsync()
    {
        await dbContainer.DisposeAsync();

        provider?.Dispose();
        fixtureScope?.Dispose();
    }
}
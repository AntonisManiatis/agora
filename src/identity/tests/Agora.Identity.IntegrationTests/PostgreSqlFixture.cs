using Agora.Identity.Infrastructure;
using Agora.Identity.Infrastructure.Data;
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

    internal UserService UserService => fixtureScope!.ServiceProvider!.GetRequiredService<UserService>();
    internal IDbConnector Connector => provider!.GetRequiredService<IDbConnector>();

    public async Task InitializeAsync()
    {
        await dbContainer.StartAsync();

        var cs = dbContainer.GetConnectionString();

        var services = new ServiceCollection();
        services.AddPostgreSql(cs);
        services.TryAddMessaging();
        services.AddIdentityMigrations(cs);

        services.Add();
        services.AddIdentity();

        provider = services.BuildServiceProvider(validateScopes: true);
        fixtureScope = provider.CreateScope();

        var connector = provider.GetRequiredService<IDbConnector>();
        using var connection = await connector.ConnectAsync();

        // TODO: Will fail if other fixtures run.
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
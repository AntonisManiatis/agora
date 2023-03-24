using Agora.Shared.Infrastructure;
using Agora.Shared.Infrastructure.Data;
using Agora.Stores.Infrastructure;

using Dapper;

using FluentMigrator.Runner;

using Microsoft.Extensions.DependencyInjection;

using Testcontainers.PostgreSql;

namespace Agora.Stores.IntegrationTests;

public class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer dbContainer =
        new PostgreSqlBuilder().Build();

    private ServiceProvider? provider;
    private IServiceScope? fixtureScope;

    internal StoreService Service => fixtureScope!.ServiceProvider.GetRequiredService<StoreService>();
    internal IDbConnector Connector => provider!.GetRequiredService<IDbConnector>();

    public async Task InitializeAsync()
    {
        await dbContainer.StartAsync();

        var cs = dbContainer.GetConnectionString();

        var services = new ServiceCollection();
        services.AddPostgreSql(cs);
        services.AddStoreMigrations(cs);
        services.AddStores();

        provider = services.BuildServiceProvider(validateScopes: true);
        fixtureScope = provider.CreateScope();

        var connector = provider.GetRequiredService<IDbConnector>();
        using var connection = await connector.ConnectAsync();

        // Fluent Migrator doesn't create a database.
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
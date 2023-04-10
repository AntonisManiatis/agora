using Agora.Shared;
using Agora.Shared.Infrastructure;
using Agora.Shared.Infrastructure.Data;
using Agora.Shared.Infrastructure.Messaging;
using Agora.Stores.Services;

using Dapper;

using Microsoft.Extensions.DependencyInjection;

using Testcontainers.PostgreSql;

namespace Agora.Stores.IntegrationTests;

public class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer dbContainer =
        new PostgreSqlBuilder().Build();

    private ServiceProvider? provider;
    private IServiceScope? fixtureScope;

    internal IStoreService StoreService => fixtureScope!.ServiceProvider.GetRequiredService<IStoreService>();
    internal IInventoryService ProductService =>
        fixtureScope!.ServiceProvider.GetRequiredService<IInventoryService>();
    internal ICategoryService CategoryService =>
        fixtureScope!.ServiceProvider.GetRequiredService<ICategoryService>();
    internal IDbConnector Connector => provider!.GetRequiredService<IDbConnector>();

    public async Task InitializeAsync()
    {
        await dbContainer.StartAsync();

        var cs = dbContainer.GetConnectionString();

        var services = new ServiceCollection();
        services.AddShared(cs);
        services.AddMigrations(cs, typeof(IStoreService).Assembly);
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
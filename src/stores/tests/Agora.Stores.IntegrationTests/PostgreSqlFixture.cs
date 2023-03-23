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

    private IServiceScope? scope;

    internal StoreService Service => scope!.ServiceProvider.GetRequiredService<StoreService>();

    public async Task InitializeAsync()
    {
        await dbContainer.StartAsync();

        var services = new ServiceCollection();
        var cs = dbContainer.GetConnectionString();

        services.AddStores(cs);
        services.AddStoreMigrations(cs);

        var provider = services.BuildServiceProvider();
        scope = provider.CreateScope();

        var connector = provider.GetRequiredService<IDbConnector>();
        var connection = await connector.ConnectAsync();

        await connection.ExecuteAsync(@"
            CREATE DATABASE agora;
            CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";
        ");

        using (var scope = provider.CreateScope())
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }
    }

    public async Task DisposeAsync()
    {
        await dbContainer.DisposeAsync();

        scope?.Dispose();
    }
}
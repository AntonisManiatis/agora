using System.Reflection;

using Agora.Shared.Infrastructure;
using Agora.Shared.Infrastructure.Data;

using Dapper;

using Microsoft.Extensions.DependencyInjection;

using Testcontainers.PostgreSql;

namespace Agora.Shared.IntegrationTests;

/// <summary>
/// Spins up a PostgreSql docker container and creates a database called agora. 
/// </summary>
public abstract class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer dbContainer =
        new PostgreSqlBuilder().Build();

    private ServiceProvider? provider;

    protected abstract Assembly[] Migrations { get; }

    public string? ConnectionString { get; private set; }

    public async Task InitializeAsync()
    {
        await dbContainer.StartAsync();

        ConnectionString = dbContainer.GetConnectionString();

        var services = new ServiceCollection();
        services.AddShared(ConnectionString);
        services.AddMigrations(ConnectionString, Migrations);

        provider = services.BuildServiceProvider(validateScopes: true);

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
    }
}
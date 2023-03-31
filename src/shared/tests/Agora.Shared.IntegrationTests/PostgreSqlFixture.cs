/*
using Agora.Shared.Infrastructure;
using Agora.Shared.Infrastructure.Data;
using Agora.Shared.Infrastructure.Messaging;

using Dapper;

using Microsoft.Extensions.DependencyInjection;

using Testcontainers.PostgreSql;

namespace Agora.Stores.IntegrationTests;

public class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer dbContainer =
        new PostgreSqlBuilder().Build();

    private ServiceProvider? provider;

    internal IDbConnector Connector => provider!.GetRequiredService<IDbConnector>();

    public async Task InitializeAsync()
    {
        await dbContainer.StartAsync();

        var cs = dbContainer.GetConnectionString();

        var services = new ServiceCollection();
        services.AddPostgreSql(cs);
        services.TryAddMessaging();

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
*/
using Agora.Shared.Infrastructure.Data;

using Dapper;

using Microsoft.Extensions.DependencyInjection;

using Testcontainers.PostgreSql;

namespace Agora.Shared.IntegrationTests;

/// <summary>
/// Spins up a PostgreSql docker container and creates a database called agora. 
/// </summary>
public sealed class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer dbContainer =
        new PostgreSqlBuilder().Build();

    public string? ConnectionString { get; private set; }

    public async Task InitializeAsync()
    {
        await dbContainer.StartAsync();

        var connectionString = dbContainer.GetConnectionString();
        ConnectionString = connectionString;

        var services = new ServiceCollection();
        services.AddPostgreSql(connectionString);

        using (var provider = services.BuildServiceProvider(validateScopes: true))
        {
            using (var scope = provider.CreateScope())
            {
                var connector = scope.ServiceProvider.GetRequiredService<IDbConnector>();
                using var connection = await connector.ConnectAsync();

                await connection.ExecuteAsync(@"
                    CREATE DATABASE agora;
                    CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";
                ");
            }
        }
    }

    public async Task DisposeAsync() => await dbContainer.DisposeAsync();
}
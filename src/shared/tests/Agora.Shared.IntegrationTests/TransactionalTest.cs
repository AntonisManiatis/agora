using Agora.Shared.Infrastructure.Data;
using Agora.Shared.Infrastructure.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using Testcontainers.PostgreSql;

namespace Agora.Shared.IntegrationTests;

public class PostgreSql : IAsyncLifetime
{
    private readonly PostgreSqlContainer dbContainer =
        new PostgreSqlBuilder().Build();

    internal string? ConnectionString { get; private set; }

    public async Task InitializeAsync()
    {
        await dbContainer.StartAsync();

        ConnectionString = dbContainer.GetConnectionString();
    }

    public async Task DisposeAsync() => await dbContainer.DisposeAsync();
}

public class TransactionalTest : IClassFixture<PostgreSql>
{
    private readonly PostgreSql fixture;

    public TransactionalTest(PostgreSql fixture) => this.fixture = fixture;

    [Fact]
    public async Task TestAsync()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddShared(fixture.ConnectionString!);

        // ! looks like interfaces work better with Castle.
        services.AddProxiedScoped<ITransactionalService, TransactionalService>();

        var container = services.BuildServiceProvider();

        var outterConnector = container.GetService<IDbConnector>();

        // Act
        using (var scope = container.CreateScope())
        {
            // Assert
            var service = container.GetService<ITransactionalService>();
            await service!.DoWork();
        }
    }
}

interface ITransactionalService
{
    Task DoWork();
}

class TransactionalService : ITransactionalService
{
    private readonly IDbConnector connector;

    public TransactionalService(IDbConnector connector) =>
        this.connector = connector;

    public Task DoWork()
    {
        return Task.CompletedTask;
    }
}
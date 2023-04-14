using System.Data;

using Agora.Shared.Infrastructure.Data;
using Agora.Shared.Infrastructure.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Npgsql;

namespace Agora.Shared.IntegrationTests;

public class TransactionalTest : IClassFixture<PostgreSqlFixture>
{
    private readonly PostgreSqlFixture fixture;

    public TransactionalTest(PostgreSqlFixture fixture) => this.fixture = fixture;

    [Fact]
    public async Task TestAsync()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddShared(fixture.ConnectionString!);
        services.AddScoped<Decorator>(_ => new Decorator(fixture.ConnectionString!));
        services.RemoveAll<IDbConnector>();
        services.AddScoped<IDbConnector, Decorator>(_ => new Decorator(fixture.ConnectionString!));
        // ! looks like interfaces work better with Castle.

        services.AddProxiedScoped<ITransactionalService, TransactionalService>();

        using var container = services.BuildServiceProvider();

        // Act
        using (var scope = container.CreateScope())
        {
            // Assert
            var service = container.GetService<ITransactionalService>();
            await service!.DoWork();
        }
    }
}

sealed class Decorator : IDbConnector
{
    private readonly TransactionDbConnection connection;

    public Decorator(string connectionString) =>
        this.connection = new TransactionDbConnection(new NpgsqlConnection(connectionString));

    public Task<IDbConnection> ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        return Task.FromResult<IDbConnection>(connection);
    }
}

sealed class TransactionDbConnection : IDbConnection
{
    private readonly IDbConnection inner;

    public TransactionDbConnection(IDbConnection inner) => this.inner = inner;

    public string ConnectionString
    {
        get => inner.ConnectionString;
#nullable disable // ! I get a warning I don't understand and I have warnings as errors XD
        set => inner.ConnectionString = value;
#nullable enable
    }

    public int ConnectionTimeout => inner.ConnectionTimeout;

    public string Database => inner.Database;

    public ConnectionState State => inner.State;

    public IDbTransaction? Transaction { get; private set; }

    public IDbTransaction BeginTransaction()
    {
        var transaction = inner.BeginTransaction();
        Transaction = transaction;
        return transaction;
    }

    public IDbTransaction BeginTransaction(IsolationLevel il)
    {
        var transaction = inner.BeginTransaction(il);
        Transaction = transaction;
        return transaction;
    }

    public void ChangeDatabase(string databaseName) => inner.ChangeDatabase(databaseName);

    public void Close() => inner.Close();

    public IDbCommand CreateCommand() => inner.CreateCommand();

    public void Dispose() => inner.Dispose();

    public void Open() => inner.Open();
}

interface ITransactionalService
{
    Task DoWork();
}

class TransactionalService : ITransactionalService
{
    private readonly Decorator connector;

    public TransactionalService(Decorator connector) =>
        this.connector = connector;

    [Transactional]
    public async Task DoWork()
    {
        var connection = await connector.ConnectAsync();
        var tc = connection as TransactionDbConnection;
        Assert.NotNull(tc!.Transaction);
    }
}
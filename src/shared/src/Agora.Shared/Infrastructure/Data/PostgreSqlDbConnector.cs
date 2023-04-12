using System.Data;

using Npgsql;

namespace Agora.Shared.Infrastructure.Data;

sealed class PostgreSqlDbConnector : IDbConnector, IDisposable
{
    private readonly string connectionString;
    private readonly Lazy<Task<IDbConnection>> holder;

    public PostgreSqlDbConnector(string connectionString)
    {
        this.connectionString = connectionString;
        this.holder = new Lazy<Task<IDbConnection>>(async () =>
        {
            var connection = new NpgsqlConnection(connectionString);
            // TODO: I need: https://devblogs.microsoft.com/pfxteam/asynclazyt/
            await connection.OpenAsync();
            return connection;
        });
    }

    public async Task<IDbConnection> ConnectAsync(CancellationToken cancellationToken = default)
    {
        var connection = new NpgsqlConnection(connectionString);
        // TODO: I need: https://devblogs.microsoft.com/pfxteam/asynclazyt/
        await connection.OpenAsync(cancellationToken);
        return connection;
    }

    public void Dispose()
    {
        // TODO: Dispose the connection
    }
}
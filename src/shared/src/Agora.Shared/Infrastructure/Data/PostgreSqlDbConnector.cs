using System.Data;

using Npgsql;

namespace Agora.Shared.Infrastructure.Data;

sealed class PostgreSqlDbConnector : IDbConnector, IDisposable, IAsyncDisposable
{
    private readonly NpgsqlConnection connection;

    public PostgreSqlDbConnector(string connectionString) =>
        this.connection = new NpgsqlConnection(connectionString);

    public async Task<IDbConnection> ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        return connection;
    }

    public void Dispose() => connection.Dispose();

    public ValueTask DisposeAsync() => connection.DisposeAsync();

}
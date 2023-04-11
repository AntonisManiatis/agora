using System.Data;

using Npgsql;

namespace Agora.Shared.Infrastructure.Data;

sealed class PostgreSqlDbConnector : IDbConnector
{
    private readonly string connectionString;

    public PostgreSqlDbConnector(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task<IDbConnection> ConnectAsync(CancellationToken cancellationToken = default)
    {
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
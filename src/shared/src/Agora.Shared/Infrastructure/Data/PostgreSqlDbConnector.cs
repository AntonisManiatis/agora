using System.Data;

using Npgsql;

namespace Agora.Shared.Infrastructure.Data;

internal sealed class PostgreSqlDbConnector : IDbConnector
{
    private readonly string connectionString;

    public PostgreSqlDbConnector(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task<IDbConnection> ConnectAsync() // ? Maybe ct?
    {
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        return connection;
    }
}
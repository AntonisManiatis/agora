using Agora.Catalogs.Infrastructure.Data.Entities;
using Agora.Shared.Infrastructure.Data;

using Dapper;

namespace Agora.Catalogs.Infrastructure.Data;

sealed class PostgreSqlStoreRepository : IStoreRepository
{
    private readonly IDbConnector connector;

    public PostgreSqlStoreRepository(IDbConnector connector) => this.connector = connector;

    public async Task<bool> ExistsAsync(string storeName)
    {
        var connection = await connector.ConnectAsync();

        var exists = await connection.ExecuteScalarAsync<bool>(
            $"SELECT COUNT(1) FROM {Sql.Schema}.{Sql.Store.Table} WHERE {Sql.Store.Name}=@Name",
            new { Name = storeName }
        );

        return exists;
    }

    public async Task SaveAsync(Store store)
    {
        var connection = await connector.ConnectAsync();

        _ = await connection.ExecuteAsync(
            $@"INSERT INTO {Sql.Schema}.{Sql.Store.Table}
            ({Sql.Store.Id}, {Sql.Store.Name}, {Sql.Store.Lang})
            VALUES (@{nameof(Store.Id)}, @{nameof(Store.Name)}, @{nameof(Store.Lang)})",
            store
        );
    }
}
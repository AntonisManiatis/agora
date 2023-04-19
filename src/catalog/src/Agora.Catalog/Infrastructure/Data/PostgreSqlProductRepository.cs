
using Agora.Catalog.Infrastructure.Data.Entities;
using Agora.Shared.Infrastructure.Data;

using Dapper;

namespace Agora.Catalog.Infrastructure.Data;

sealed class PostgreSqlProductRepository : IProductRepository
{
    private readonly IDbConnector connector;

    public PostgreSqlProductRepository(IDbConnector connector) => this.connector = connector;

    public async Task SaveAsync(Product product)
    {
        var connection = await connector.ConnectAsync();

        await connection.ExecuteAsync(
            $@"INSERT INTO {Sql.Schema}.{Product.Schema.Table}
            ({Product.Schema.Id}, {Product.Schema.Title}, {Product.Schema.Description})
            VALUES (@{nameof(Product.Id)}, @{nameof(Product.Title)}, @{nameof(Product.Description)})",
            product
        );
    }
}
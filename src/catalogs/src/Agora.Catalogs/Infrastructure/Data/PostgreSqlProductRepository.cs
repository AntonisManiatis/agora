
using Agora.Catalogs.Infrastructure.Data.Entities;
using Agora.Shared.Infrastructure.Data;

using Dapper;

namespace Agora.Catalogs.Infrastructure.Data;

sealed class PostgreSqlProductRepository : IProductRepository
{
    private readonly IDbConnector connector;

    public PostgreSqlProductRepository(IDbConnector connector) => this.connector = connector;

    public async Task SaveAsync(Product product)
    {
        var connection = await connector.ConnectAsync();

        await connection.ExecuteAsync(
            $@"INSERT INTO {Sql.Schema}.{Sql.Product.Table}
            ({Sql.Product.Id}, {Sql.Product.Title}, {Sql.Product.Description})
            VALUES (@Id, @Title, @Description)",
            new
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description
            }
        );
    }
}
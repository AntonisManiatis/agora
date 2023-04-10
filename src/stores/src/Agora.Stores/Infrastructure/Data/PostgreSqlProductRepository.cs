using Agora.Shared.Infrastructure.Data;
using Agora.Stores.Core.Products;

using Dapper;

namespace Agora.Stores.Infrastructure.Data;

sealed class PostgreSqlProductRepository : IProductRepository
{
    private readonly IDbConnector connector;

    public PostgreSqlProductRepository(IDbConnector connector)
    {
        this.connector = connector;
    }

    public async Task AddAsync(Product product)
    {
        using var connection = await connector.ConnectAsync();

        // TODO: What if a product already exists?

        // ! Transaction is an application concern, we'll get back to this.
        using var transaction = connection.BeginTransaction();

        _ = await connection.ExecuteAsync(
            $@"INSERT INTO {Sql.Schema}.{Sql.Product.Table}
            ({Sql.Product.Id}, {Sql.Product.StoreId}, {Sql.Product.Sku}, {Sql.Product.Quantity})
            VALUES (@Id, @StoreId, @Sku, @Quantity)",
            new
            {
                Id = product.Id.Value,
                StoreId = product.StoreId.Value,
                Sku = product.Sku,
                Quantity = product.Quantity.Value
            },
            transaction
        );

        transaction.Commit();
    }

    public async Task<Product?> GetAsync(ProductId productId)
    {
        using var connection = await connector.ConnectAsync();

        var id = new { Id = productId.Value };
        // TODO: What if this is null?
        var productEntity = await connection.QueryFirstAsync<ProductEntity>(
            $@"SELECT * FROM {Sql.Schema}.{Sql.Product.Table}
            WHERE {Sql.Product.Id}=@Id",
            id
        );

        return new Product(
            productId,
            productEntity.StoreId,
            productEntity.Sku,
            productEntity.Quantity
        );
    }
}
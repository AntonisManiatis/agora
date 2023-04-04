using Agora.Shared.Infrastructure.Data;
using Agora.Stores.Core;

using Dapper;

namespace Agora.Stores.Infrastructure.Data;

internal sealed class PostgreSqlStoreRepository : IStoreRepository
{
    private readonly IDbConnector connector;

    public PostgreSqlStoreRepository(IDbConnector connector)
    {
        this.connector = connector;
    }

    public async Task<Guid> AddAsync(Store store)
    {
        using var connection = await connector.ConnectAsync();

        if (store.Id == Guid.Empty)
        {
            var storeId = await connection.ExecuteScalarAsync<Guid>(
                $"INSERT INTO stores.{Sql.Stores.Table} (user_id, name, status, tin) VALUES (@UserId, @Name, @Status, @Tin) RETURNING id",
                new
                {
                    UserId = store.UserId,
                    Name = store.Name,
                    Status = store.Status,
                    Tin = store.Tin
                }
            );

            return storeId;
        }

        using var transaction = connection.BeginTransaction();
        // Update the store. 
        await connection.ExecuteAsync(
            $"UPDATE stores.{Sql.Stores.Table} SET user_id=@UserId, name=@Name, status=@Status, tin=@Tin WHERE id=@Id",
            new
            {
                Id = store.Id,
                UserId = store.UserId,
                Name = store.Name,
                Status = store.Status,
                Tin = store.Tin
            },
            transaction
        );

        // EF core has an api that tracks changes but with dapper there are other options:
        // https://stackoverflow.com/questions/67192842/how-to-use-dapper-with-change-tracking-to-save-an-altered-list

        // for this case since it's the easiest and not done often I'll delete the entire list and re-insert.
        await connection.ExecuteAsync($"DELETE FROM stores.store_category WHERE store_id=@StoreId",
            new { StoreId = store.Id },
            transaction
        );

        foreach (var category in store.Categories)
        {
            // Not very efficient.
            int categoryId = await connection.ExecuteScalarAsync<int>(
                "INSERT INTO stores.store_category (store_id, name) VALUES (@StoreId, @Name) RETURNING id",
                new { StoreId = store.Id, Name = category.Name },
                transaction
            );

            category.Id = categoryId;
        }

        // TODO: Categories have products also.

        // TODO: I'm not sure if this should directly return an Id. 
        return store.Id;
    }

    public async Task<bool> ExistsAsync(string storeName)
    {
        using var connection = await connector.ConnectAsync();

        var exists = await connection.ExecuteScalarAsync<bool>(
            $"SELECT COUNT(1) FROM stores.{Sql.Stores.Table} WHERE name=@Name",
            new { Name = storeName }
        );

        return exists;
    }

    public async Task<Store?> GetStoreAsync(Guid storeId)
    {
        using var connection = await connector.ConnectAsync();

        var store = await connection.QueryFirstOrDefaultAsync<Store?>(
            $"SELECT * FROM stores.{Sql.Stores.Table} WHERE id = @Id",
            new { Id = storeId }
        );

        if (store is not null)
        {
            // Perhaps needed here. 
            // https://andrewlock.net/using-snake-case-column-names-with-dapper-and-postgresql/
            var categories = await connection.QueryAsync<Category>(
                $"SELECT * FROM stores.store_category WHERE store_id = @Id",
                new { Id = store.Id }
            );

            foreach (var category in categories)
            {
                store.AddCategory(category);
            }
        }

        return store;
    }

    public Task<IEnumerable<Store>> GetStoresAsync()
    {
        throw new NotImplementedException();
    }
}
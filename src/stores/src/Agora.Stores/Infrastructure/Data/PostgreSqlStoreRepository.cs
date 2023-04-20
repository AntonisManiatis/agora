using Agora.Shared.Infrastructure.Data;
using Agora.Stores.Core.Stores;

using Dapper;

namespace Agora.Stores.Infrastructure.Data;

sealed class PostgreSqlStoreRepository : IStoreRepository
{
    private readonly IDbConnector connector;

    public PostgreSqlStoreRepository(IDbConnector connector) => this.connector = connector;

    public async Task AddAsync(Store store)
    {
        var connection = await connector.ConnectAsync();

        // Update the store.
        var storeId = store.Id.Value;
        await connection.ExecuteAsync(
            $@"INSERT INTO {Sql.Schema}.{Sql.Stores.Table}
            ({Sql.Stores.Id}, {Sql.Stores.OwnerId}, {Sql.Stores.Status}, {Sql.Stores.Tin})
            VALUES (@Id, @OwnerId, @Status, @Tin)",
            new
            {
                Id = storeId,
                OwnerId = store.OwnerId.Value,
                store.Status,
                store.Tin
            }
        );

        // EF core has an api that tracks changes but with dapper there are other options:
        // https://stackoverflow.com/questions/67192842/how-to-use-dapper-with-change-tracking-to-save-an-altered-list

        // for this case since it's the easiest and not done often I'll delete the entire list and re-insert.
        await connection.ExecuteAsync($"DELETE FROM {Sql.Schema}.{Sql.Category.Table} WHERE {Sql.Category.StoreId}=@StoreId",
            new { StoreId = storeId }
        );

        foreach (var category in store.Categories)
        {
            // Not very efficient.
            // If I could batch it somehow and return all ids?
            category.Id = await connection.ExecuteScalarAsync<int>(
                $@"INSERT INTO {Sql.Schema}.{Sql.Category.Table} ({Sql.Category.StoreId}, {Sql.Category.Name}) 
                VALUES (@StoreId, @Name) RETURNING {Sql.Category.Id}",
                new { StoreId = storeId, category.Name }
            );

            // TODO: Categories have products also.
        }
    }

    public async Task<Store?> GetStoreAsync(Guid storeId)
    {
        var connection = await connector.ConnectAsync();

        var id = new { Id = storeId };
        var storeEntity = await connection.QueryFirstOrDefaultAsync<StoreEntity?>(
            $"SELECT * FROM {Sql.Schema}.{Sql.Stores.Table} WHERE {Sql.Stores.Id} = @Id",
            id
        );

        if (storeEntity is null)
        {
            return null;
        }

        /*
        if (store is not null)
        {
            var categories = await connection.QueryAsync<Category>(
                $@"SELECT * FROM {Sql.Schema}.{Sql.Category.Table} WHERE {Sql.Category.StoreId} = @Id",
                id
            );

            foreach (var category in categories)
            {
                store.AddCategory(category);
            }
        }
        */

        var store = new Store(
            storeId,
            storeEntity.OwnerId
        )
        {
            Tin = storeEntity.Tin
        };
        // TODO: status too
        return store;
    }

    public Task<IEnumerable<Store>> GetStoresAsync()
    {
        throw new NotImplementedException();
    }
}
using Agora.Shared.Infrastructure.Data;
using Agora.Stores.Core.Stores;

using Dapper;

namespace Agora.Stores.Infrastructure.Data;

sealed class PostgreSqlStoreRepository : IStoreRepository
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
                $@"INSERT INTO {Sql.Schema}.{Sql.Stores.Table} 
                ({Sql.Stores.UserId}, {Sql.Stores.Name}, {Sql.Stores.Status}, {Sql.Stores.Tin})
                VALUES (@UserId, @Name, @Status, @Tin) RETURNING {Sql.Stores.Id}",
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
            $@"UPDATE {Sql.Schema}.{Sql.Stores.Table}
            SET {Sql.Stores.UserId}=@UserId, {Sql.Stores.Name}=@Name, {Sql.Stores.Status}=@Status, {Sql.Stores.Tin}=@Tin 
            WHERE {Sql.Stores.Id}=@Id",
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
        await connection.ExecuteAsync($"DELETE FROM {Sql.Schema}.{Sql.Category.Table} WHERE {Sql.Category.StoreId}=@StoreId",
            new { StoreId = store.Id },
            transaction
        );

        foreach (var category in store.Categories)
        {
            // Not very efficient.
            // If I could batch it somehow and return all ids?
            int categoryId = await connection.ExecuteScalarAsync<int>(
                $@"INSERT INTO {Sql.Schema}.{Sql.Category.Table} ({Sql.Category.StoreId}, {Sql.Category.Name}) 
                VALUES (@StoreId, @Name) RETURNING {Sql.Category.Id}",
                new { StoreId = store.Id, Name = category.Name },
                transaction
            );

            category.Id = categoryId;

            // TODO: Categories have products also.
        }

        transaction.Commit();
        // TODO: I'm not sure if this should directly return an Id. 
        return store.Id;
    }

    public async Task<bool> ExistsAsync(string storeName)
    {
        using var connection = await connector.ConnectAsync();

        var exists = await connection.ExecuteScalarAsync<bool>(
            $"SELECT COUNT(1) FROM {Sql.Schema}.{Sql.Stores.Table} WHERE {Sql.Stores.Name}=@Name",
            new { Name = storeName }
        );

        return exists;
    }

    public async Task<Store?> GetStoreAsync(Guid storeId)
    {
        using var connection = await connector.ConnectAsync();

        var id = new { Id = storeId };
        var store = await connection.QueryFirstOrDefaultAsync<Store?>(
            $"SELECT * FROM {Sql.Schema}.{Sql.Stores.Table} WHERE {Sql.Stores.Id} = @Id",
            id
        );

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

        return store;
    }

    public Task<IEnumerable<Store>> GetStoresAsync()
    {
        throw new NotImplementedException();
    }
}
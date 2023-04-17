using Agora.Catalog.Infrastructure.Data.Entities;
using Agora.Shared.Infrastructure.Data;

using Dapper;

namespace Agora.Catalog.Infrastructure.Data;

sealed class PostgreSqlCategoryRepository : ICategoryRepository
{
    private readonly IDbConnector connector;

    public PostgreSqlCategoryRepository(IDbConnector connector) => this.connector = connector;

    public async Task<bool> ExistsAsync(int id)
    {
        var connection = await connector.ConnectAsync();

        return await connection.ExecuteScalarAsync<bool>(
            @$"SELECT COUNT(1) FROM {Sql.Schema}.{Sql.Category.Table} WHERE {Sql.Category.Id}=id", id
        );
    }

    public async Task<Category?> GetAsync(int id)
    {
        var connection = await connector.ConnectAsync();

        return await connection.QueryFirstOrDefaultAsync<Category?>(
            @$"SELECT * FROM {Sql.Schema}.{Sql.Category.Table} WHERE {Sql.Category.Id}=id",
            id
        );
    }

    public async Task<int> NextIdentity()
    {
        var connection = await connector.ConnectAsync();

        return await connection.ExecuteScalarAsync<int>($"SELECT nextval('{Sql.Schema}.{Sql.Category.Table}_{Sql.Category.Id}_seq')");
    }

    public async Task SaveAsync(Category category)
    {
        var connection = await connector.ConnectAsync();

        _ = await connection.ExecuteAsync(
            $@"INSERT INTO {Sql.Schema}.{Sql.Category.Table}
            ({Sql.Category.Id}, {Sql.Category.Name}, {Sql.Category.Description}, {Sql.Category.ParentId})
            VALUES (@Id, @Name, @Description, @ParentId)
            ",
            new // ? Can't I just use the category instead?
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentId = category.ParentId
            }
        );

        // TODO: Insert attributes/options too.
    }

    public Task DeleteAsync(Category category)
    {
        throw new NotImplementedException();
    }
}
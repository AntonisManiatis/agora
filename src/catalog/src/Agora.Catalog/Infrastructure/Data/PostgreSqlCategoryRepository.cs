using Agora.Catalog.Infrastructure.Data.Entities;
using Agora.Shared;
using Agora.Shared.Infrastructure.Data;

using Dapper;

using ErrorOr;

using Npgsql;

namespace Agora.Catalog.Infrastructure.Data;

sealed class PostgreSqlCategoryRepository : ICategoryRepository
{
    private readonly IDbConnector connector;

    public PostgreSqlCategoryRepository(IDbConnector connector) => this.connector = connector;

    public async Task<bool> ExistsAsync(int id)
    {
        var connection = await connector.ConnectAsync();

        return await connection.ExecuteScalarAsync<bool>(
            @$"SELECT COUNT(1) FROM {Sql.Schema}.{Category.Schema.Table}
            WHERE {Category.Schema.Id}=@{nameof(Category.Id)}",
            new { Id = id }
        );
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        var connection = await connector.ConnectAsync();

        var categories = await connection.QueryAsync<Category>(
            @$"SELECT * FROM {Sql.Schema}.{Category.Schema.Table}"
        );

        var indexed = categories.ToDictionary(c => c.Id);
        foreach (var category in categories)
        {
            if (category.ParentId is int parentId)
            {
                var cat = indexed[parentId];
                if (cat is null)
                {
                    continue;
                }

                var children = cat?.Children ?? new List<Category>();

                children.Add(category);

                if (cat!.Children is null)
                {
                    cat!.Children = children;
                }
            }
        }

        return categories.Where(c => c.ParentId is null).ToList();
    }

    public async Task<Category?> GetAsync(int id)
    {
        var connection = await connector.ConnectAsync();

        var categories = await connection.QueryAsync<Category>(
            @$"SELECT * FROM {Sql.Schema}.{Category.Schema.Table}"
        );

        var indexed = categories.ToDictionary(c => c.Id);
        foreach (var category in categories)
        {
            if (category.ParentId is int parentId)
            {
                var cat = indexed[parentId];
                if (cat is null)
                {
                    continue;
                }

                var children = cat?.Children ?? new List<Category>();

                children.Add(category);

                if (cat!.Children is null)
                {
                    cat!.Children = children;
                }
            }
        }

        return indexed.GetValueOrDefault(id);
    }

    public async Task<int> NextIdentity()
    {
        var connection = await connector.ConnectAsync();

        return await connection.ExecuteScalarAsync<int>(
            $"SELECT nextval('{Sql.Schema}.{Category.Schema.Table}_{Category.Schema.Id}_seq')"
        );
    }

    public async Task<ErrorOr<Unit>> SaveAsync(Category category)
    {
        var connection = await connector.ConnectAsync();

        try
        {
            _ = await connection.ExecuteAsync(
                $@"INSERT INTO {Sql.Schema}.{Category.Schema.Table}
                ({Category.Schema.Id}, {Category.Schema.Name}, {Category.Schema.Description}, {Category.Schema.ParentId})
                VALUES (@{nameof(Category.Id)}, @{nameof(Category.Name)}, @{nameof(Category.Description)}, @{nameof(Category.ParentId)})",
                category
            );
        }
        catch (PostgresException pe)
        {
            return pe.SqlState switch
            {
                // * We can safely assume it's the category name.
                // ? Maybe move this error to another location because currently it's duplicated.
                PostgresErrorCodes.UniqueViolation => Error.Conflict(code: "Caregory.AlreadyExists", description: "Category already exists."),
                _ => Error.Failure()
            };
        }

        if (category.Attributes is not []) // I assume this is a "and null" pattern
        {
            // TODO: Insert attributes/options too.
            // _ = await connection.ExecuteAsync(
            //     $@"INSERT INTO {Sql.Schema}.{""}
            //     ()
            //     VALUES ()
            //     "
            // );
        }
        return new Unit();
    }

    public Task DeleteAsync(Category category)
    {
        throw new NotImplementedException();
    }
}
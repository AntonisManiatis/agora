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
            $"SELECT * FROM {Sql.Schema}.{Category.Schema.Table}"
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

    public async Task<Category?> GetAsync(int id, bool loadAttributes = false)
    {
        var connection = await connector.ConnectAsync();

        // * Seems to fix duplicates.
        // https://stackoverflow.com/questions/55557403/how-to-map-objects-when-using-dapper-spliton

        Dictionary<int, Category> indexed;
        IEnumerable<Category> categories;
        if (loadAttributes)
        {
            var sql =
                $@"
                SELECT 
                    c.*, pa.*, po.*
                FROM 
                    {Sql.Schema}.{Category.Schema.Table} c
                LEFT JOIN 
                    {Sql.Schema}.{ProductAttribute.Schema.Table} pa ON c.{Category.Schema.Id} = pa.{ProductAttribute.Schema.CategoryId}
                LEFT JOIN 
                    {Sql.Schema}.{ProductOption.Schema.Table} po ON pa.{ProductAttribute.Schema.Id} = po.{ProductOption.Schema.AttributeId}
                WHERE
                    c.{Category.Schema.Id}=@{nameof(Category.Id)}
                ";

            indexed = new Dictionary<int, Category>();
            var attributes = new Dictionary<int, ProductAttribute>();
            var options = new Dictionary<int, ProductOption>();

            categories = await connection.QueryAsync<Category, ProductAttribute, ProductOption, Category>(
                sql,
                (category, attribute, option) =>
                {
                    if (!indexed.TryGetValue(category.Id, out Category? cat))
                    {
                        indexed[category.Id] = category;
                        cat = category;
                    }

                    if (!attributes.TryGetValue(attribute.Id, out ProductAttribute? attr))
                    {
                        attributes[attribute.Id] = attribute;
                        attr = attribute;
                        cat.Attributes.Add(attr);
                    }

                    if (!options.TryGetValue(option.Id, out ProductOption? opt))
                    {
                        options[option.Id] = option;
                        opt = option;
                        attr.Options.Add(opt);
                    }

                    return cat;
                },
                new { Id = id },
                splitOn: "id,id,id"
            );
        }
        else
        {
            categories = await connection.QueryAsync<Category>(
                $"SELECT * FROM {Sql.Schema}.{Category.Schema.Table}"
            );

            indexed = categories.ToDictionary(c => c.Id);
        }

        foreach (var category in categories)
        {
            if (category.ParentId is int parentId)
            {
                if (indexed.TryGetValue(category.Id, out var cat))
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
        ArgumentNullException.ThrowIfNull(category);

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

        // TODO: deal with list changes.
        // ? I have no idea if this can be done more efficiently.
        foreach (var attribute in category.Attributes)
        {
            var attributeId = await connection.ExecuteScalarAsync<int>(
                $@"INSERT INTO {Sql.Schema}.{ProductAttribute.Schema.Table}
                ({ProductAttribute.Schema.CategoryId}, {ProductAttribute.Schema.Name}, {ProductAttribute.Schema.PickOne})
                VALUES (@{nameof(ProductAttribute.CategoryId)}, @{nameof(ProductAttribute.Name)}, @{nameof(ProductAttribute.PickOne)})
                RETURNING {ProductAttribute.Schema.Id}",
                attribute
            );

            foreach (var option in attribute.Options)
            {
                option.AttributeId = attributeId;

                option.Id = await connection.ExecuteScalarAsync<int>(
                    $@"INSERT INTO {Sql.Schema}.{ProductOption.Schema.Table}
                    ({ProductOption.Schema.AttributeId}, {ProductOption.Schema.Name})
                    VALUES (@{nameof(ProductOption.AttributeId)}, @{nameof(ProductOption.Name)})
                    RETURNING {ProductOption.Schema.Id}",
                    option
                );
            }
        }

        return new Unit();
    }

    public async Task DeleteAsync(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        var connection = await connector.ConnectAsync();

        // ? Cascades?
        _ = await connection.ExecuteAsync(
            $@"DELETE FROM {Sql.Schema}.{Category.Schema.Table}
            WHERE {Category.Schema.Id}=@{nameof(Category.Id)}",
            new { category.Id }
        );
    }
}
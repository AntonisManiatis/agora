namespace Agora.Catalogs.Infrastructure.Data;

static class Sql
{
    internal const string Schema = "catalogs";

    internal static class Product
    {
        internal const string Table = "product";

        internal static readonly string Id = nameof(Product.Id).ToSnakeCase();
        internal static readonly string Title = nameof(Product.Title).ToSnakeCase();
        internal static readonly string Description = nameof(Product.Description).ToSnakeCase();
    }
}
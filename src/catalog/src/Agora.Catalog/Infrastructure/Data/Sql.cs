namespace Agora.Catalog.Infrastructure.Data;

static class Sql
{
    internal const string Schema = "catalog";

    internal static class Category
    {
        internal const string Table = "category";

        internal static string Id = nameof(Category.Id).ToSnakeCase();
        internal static string Name = nameof(Category.Name).ToSnakeCase();
        internal static string Description = nameof(Category.Description).ToSnakeCase();
        internal static string ImageUrl = nameof(Category.ImageUrl).ToSnakeCase();
        internal static string ParentId = nameof(Category.ParentId).ToSnakeCase();
    }

    internal static class Product
    {
        internal const string Table = "product";

        internal static readonly string Id = nameof(Product.Id).ToSnakeCase();
        internal static readonly string Title = nameof(Product.Title).ToSnakeCase();
        internal static readonly string Description = nameof(Product.Description).ToSnakeCase();
    }

    internal static class Store
    {
        internal const string Table = "store";

        internal static readonly string Id = nameof(Store.Id).ToSnakeCase();
        internal static readonly string Name = nameof(Store.Name).ToSnakeCase();
        internal static readonly string Lang = nameof(Store.Lang).ToSnakeCase();
    }
}
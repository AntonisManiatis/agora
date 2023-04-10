using Agora.Stores.Core.Stores;

namespace Agora.Stores.Infrastructure.Data;

internal static class Sql
{
    internal const string Schema = "stores";

    internal static class Stores
    {
        internal const string Table = "store";

        internal static readonly string Id = nameof(Store.Id).ToSnakeCase();
        internal static readonly string UserId = nameof(Store.UserId).ToSnakeCase();
        internal static readonly string Name = nameof(Store.Name).ToSnakeCase();
        internal static readonly string Status = nameof(Store.Status).ToSnakeCase();
        internal static readonly string Tin = nameof(Store.Tin).ToSnakeCase();
        // TODO: add more.
    }

    internal static class Category
    {
        internal const string Table = "store_category";

        internal static readonly string Id = nameof(Core.Stores.Category.Id).ToSnakeCase();
        internal static readonly string Name = nameof(Core.Stores.Category.Name).ToSnakeCase();
        internal static readonly string StoreId = "store_id";
    }

    internal static class Product
    {
        internal const string Table = "product";

        internal static readonly string Id = "product_id";
        internal static readonly string StoreId = nameof(Core.Products.Product.StoreId).ToSnakeCase();
        internal static readonly string Sku = nameof(Core.Products.Product.Sku).ToSnakeCase();
        internal static readonly string Quantity = nameof(Core.Products.Product.Quantity).ToSnakeCase();
    }
}
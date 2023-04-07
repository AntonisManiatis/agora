using Agora.Shared.Core;
using Agora.Stores.Core.Stores;

namespace Agora.Stores.Core.Products;

sealed class Product : Entity
{
    private readonly List<ProductOption> options = new();

    internal Product(ProductId id, StoreId storeId)
    {
        Id = id;
        StoreId = storeId;
    }

    internal ProductId Id { get; }
    internal StoreId StoreId { get; }
    internal string Title { get; set; } = string.Empty;
    internal string Description { get; set; } = string.Empty;
    internal IReadOnlyList<ProductOption> Options => options;

    internal static Product List(ProductId productId, StoreId storeId)
    {
        var product = new Product(productId, storeId);
        product.AddEvent(new ProductListed(productId, storeId));
        return product;
    }
}
using Agora.Shared.Core;
using Agora.Stores.Core.Stores;

using ErrorOr;

namespace Agora.Stores.Core.Products;

sealed class Product : Entity
{
    internal Product(
        ProductId id,
        StoreId storeId,
        string? sku,
        Quantity quantity
    )
    {
        Id = id;
        StoreId = storeId;
        Sku = sku;
        Quantity = quantity;
    }

    internal static ErrorOr<Product> List(
        ProductId productId,
        StoreId storeId,
        string? sku,
        Quantity quantity
    )
    {
        // ? others?

        var product = new Product(
            productId,
            storeId,
            sku,
            quantity
        );

        product.AddEvent(new ProductListed(productId, storeId));
        return product;
    }

    internal ProductId Id { get; }

    internal StoreId StoreId { get; }

    internal string? Sku { get; set; }

    internal Quantity Quantity { get; private set; }
    // TODO: Implement equality, either here or in Entity type?
}
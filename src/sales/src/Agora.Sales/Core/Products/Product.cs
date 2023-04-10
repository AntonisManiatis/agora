using Agora.Shared.Core;

using ErrorOr;

namespace Agora.Sales.Core.Products;

sealed class Product : Entity
{
    private readonly List<ProductVariant> variants;

    internal Product(
        ProductId id,
        // StoreId storeId,
        List<ProductVariant> variants
    )
    {
        Id = id;
        // StoreId = storeId;
        this.variants = variants;
    }

    internal static ErrorOr<Product> List(
        ProductId productId,
        // StoreId storeId,
        List<ProductVariant> variants
    )
    {
        // ? others?

        var product = new Product(
            productId,
            // storeId,
            variants
        );

        // product.AddEvent(new ProductListed(productId, storeId));
        return product;
    }

    internal ProductId Id { get; }

    // internal StoreId StoreId { get; }

    internal IReadOnlyList<ProductVariant> Variants => variants.AsReadOnly();

    // TODO: Implement equality, either here or in Entity type?
}
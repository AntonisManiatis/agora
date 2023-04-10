using Agora.Shared.Core;

namespace Agora.Sales.Core.Products;

sealed class ProductVariant : Entity
{
    internal ProductVariant(
        ProductVariantId id
    )
    {
        Id = id;
    }

    internal static ProductVariant Of(
        ProductVariantId id
    )
    {
        // TODO: validation?
        return new ProductVariant(id);
    }

    // ? Parent product id?
    internal ProductVariantId Id { get; }
}
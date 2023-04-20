namespace Agora.Sales.Core.Products;

sealed class ProductVariantId
{
    public ProductVariantId(int value)
    {
        Value = value;
    }

    internal int Value { get; }

    // TODO: Equality

    public static implicit operator int(ProductVariantId id) => id.Value;

    public static implicit operator ProductVariantId(int id) => new(id);
}
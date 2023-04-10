namespace Agora.Sales.Core.Products;

sealed class ProductId : IEquatable<ProductId>
{
    internal ProductId(Guid value)
    {
        Value = value;
    }

    internal Guid Value { get; }

    public override bool Equals(object? obj) => Equals(obj as ProductId);


    public bool Equals(ProductId? other) => other != null && Value.Equals(other.Value);


    public override int GetHashCode() => HashCode.Combine(Value);

    public static implicit operator Guid(ProductId id) => id.Value;

    public static implicit operator ProductId(Guid id) => new ProductId(id);
}
namespace Agora.Stores.Core.Products;

sealed class ProductId
{
    internal Guid Value { get; }

    internal ProductId(Guid value)
    {
        Value = value;
    }

    // TODO: Fix this.
    public override int GetHashCode() => Value.GetHashCode();

    public override bool Equals(object? obj) => Value.Equals(obj);

    public static implicit operator Guid(ProductId id) => id.Value;
    public static implicit operator ProductId(Guid id) => new ProductId(id);
}
namespace Agora.Stores.Core.Products;

sealed class ProductId
{
    internal Guid Value { get; init; }

    // TODO: Fix this.
    public override int GetHashCode() => Value.GetHashCode();

    public override bool Equals(object? obj) => Value.Equals(obj);
}
namespace Agora.Stores.Core.Products;

// TODO: Implement equality, blah blah
sealed class Quantity // I hate this being a reference type. Maybe change to struct?
{
    internal Quantity(int value)
    {
        Value = value;
    }

    internal int Value { get; }

    // TODO: Equality + factory methods

    public static implicit operator int(Quantity quantity) => quantity.Value;

    public static implicit operator Quantity(int quantity) => new(quantity);
}
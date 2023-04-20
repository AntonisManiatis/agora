namespace Agora.Stores.Core.Stores;

sealed class StoreId
{
    internal StoreId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    // TODO: Value based equality
    public static implicit operator Guid(StoreId id) => id.Value;
    public static implicit operator StoreId(Guid id) => new(id);
}
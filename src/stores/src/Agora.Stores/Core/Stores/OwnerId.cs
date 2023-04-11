namespace Agora.Stores.Core.Stores;

sealed class OwnerId
{
    // TODO: Impl value obj
    internal OwnerId(Guid value)
    {
        Value = value;
    }

    internal Guid Value { get; }

    public static implicit operator Guid(OwnerId id) => id.Value;

    public static implicit operator OwnerId(Guid id) => new OwnerId(id);
}
namespace Agora.Stores.Core;

internal enum Status
{
    Pending,
    Approved,
    Rejected
}

internal sealed class Store
{
    public Guid Id { get; set; }
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public Status Status { get; set; } = Status.Pending;
    // TODO: Introduce a value object here.
    public string Tin { get; init; } = string.Empty;
    public TaxAddress TaxAddress { get; init; } = TaxAddress.Undefined;
}
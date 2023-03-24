namespace Agora.Stores.Core;

internal sealed class StoreApplication
{
    public Guid Id { get; set; }
    public Guid UserId { get; init; }
    public Status Status { get; init; }
    public string Name { get; init; } = string.Empty;
    public Address? Address { get; init; }
}

enum Status
{
    Pending,
    Approved,
    Rejected
}
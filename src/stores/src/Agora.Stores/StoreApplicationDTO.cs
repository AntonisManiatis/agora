namespace Agora.Stores;

public record StoreApplicationDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    // ? or int?
    public string Status { get; init; } = string.Empty;
    // TODO: What else?
}
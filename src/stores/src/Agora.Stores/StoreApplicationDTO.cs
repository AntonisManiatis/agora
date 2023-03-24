namespace Agora.Stores;

public record StoreApplicationDTO
{
    public string Name { get; init; } = string.Empty;
    // ? or int?
    public string Status { get; init; } = string.Empty;
    // TODO: What else?
}
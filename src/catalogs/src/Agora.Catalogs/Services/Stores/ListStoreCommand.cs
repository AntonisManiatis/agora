namespace Agora.Catalogs.Services.Stores;

public record ListStoreCommand(
    Guid Id,
    string Name,
    string? Lang
);

// TODO: Validator
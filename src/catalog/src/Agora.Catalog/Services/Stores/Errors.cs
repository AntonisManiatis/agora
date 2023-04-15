using ErrorOr;

namespace Agora.Catalog.Services.Stores;

public static class Errors
{
    public static Error StoreAlreadyExists(string name) =>
        Error.Conflict(code: "Stores.AlreadyExists", description: $"A store named {name} already exists.");
}
using ErrorOr;

namespace Agora.Catalogs.Services.Stores;

public static class Errors
{
    public static Error StoreAlreadyExists(string name) =>
        Error.Conflict(code: "Stores.AlreadyExists", description: $"A store named {name} already exists.");
}
using ErrorOr;

namespace Agora.Stores.Services;

public static class Errors
{
    public static class Stores
    {
        public static Error NotFound =
            Error.NotFound(code: "Stores.NotFound", description: "Store not found");
    }

    public static class Categories
    {
        public static Error AlreadyExists =
            Error.Conflict(code: "Categories.AlreadyExists", description: "Category already exists");
    }
}
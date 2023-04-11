namespace Agora.Stores.Core.Stores;

sealed class Store
{
    private readonly List<Category> categories = new();

    internal Store(
        StoreId id,
        OwnerId ownerId
    )
    {
        Id = id;
        OwnerId = ownerId;
    }

    internal static Store Create(
        StoreId id,
        OwnerId ownerId
    )
    {
        return new Store(id, ownerId);
    }

    internal StoreId Id { get; }
    internal OwnerId OwnerId { get; }
    internal Status Status { get; private set; }

    // TODO: Introduce a value object here.
    internal string? Tin { get; set; }
    internal TaxAddress TaxAddress { get; init; } = TaxAddress.Undefined;

    internal IReadOnlyList<Category> Categories => categories; // TODO: AsReadOnly?

    internal bool HasCategory(string name) => categories.Any(cat => cat.Name.Equals(name));

    internal void AddCategory(string name) => categories.Add(new Category { Name = name });

    internal void AddCategory(Category category) => categories.Add(category);
}
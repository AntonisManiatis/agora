namespace Agora.Stores.Core.Stores;

enum Status
{
    Pending,
    Approved,
    Rejected
}

sealed class Store
{
    private readonly List<Category> categories = new();

    internal Guid Id { get; set; }
    internal Guid UserId { get; init; }
    internal string Name { get; init; } = string.Empty;
    internal Status Status { get; set; } = Status.Pending;
    // TODO: Introduce a value object here.
    internal string Tin { get; init; } = string.Empty;
    internal TaxAddress TaxAddress { get; init; } = TaxAddress.Undefined;
    internal IReadOnlyList<Category> Categories => categories; // TODO: AsReadOnly?

    internal bool HasCategory(string name) => categories.Any(cat => cat.Name.Equals(name));

    internal void AddCategory(string name) => categories.Add(new Category { Name = name });

    internal void AddCategory(Category category) => categories.Add(category);
}
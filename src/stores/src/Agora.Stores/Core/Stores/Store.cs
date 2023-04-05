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

    public Guid Id { get; set; }
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public Status Status { get; set; } = Status.Pending;
    // TODO: Introduce a value object here.
    public string Tin { get; init; } = string.Empty;
    public TaxAddress TaxAddress { get; init; } = TaxAddress.Undefined;
    public IReadOnlyList<Category> Categories => categories; // TODO: AsReadOnly?

    internal bool HasCategory(string name) => categories.Any(cat => cat.Name.Equals(name));

    internal void AddCategory(string name) => categories.Add(new Category { Name = name });

    internal void AddCategory(Category category) => categories.Add(category);
}
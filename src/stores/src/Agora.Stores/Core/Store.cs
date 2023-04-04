namespace Agora.Stores.Core;

enum Status
{
    Pending,
    Approved,
    Rejected
}

sealed class Store
{
    private List<Category> categories = new List<Category>();

    public Guid Id { get; set; }
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public Status Status { get; set; } = Status.Pending;
    // TODO: Introduce a value object here.
    public string Tin { get; init; } = string.Empty;
    public TaxAddress TaxAddress { get; init; } = TaxAddress.Undefined;

    public IReadOnlyList<Category> Categories => categories;

    internal bool HasCategory(string name) => Categories.Any(cat => cat.Name.Equals(name));

    internal void AddCategory(string name) => categories.Add(new Category { Name = name });

    internal void AddCategory(Category category) => categories.Add(category);
}
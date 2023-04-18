namespace Agora.Catalog.Infrastructure.Data.Entities;

class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ParentId { get; set; }
    public List<Category>? Children { get; set; }

    // TODO: attributes here.
}
using Agora.Shared;

namespace Agora.Catalog.Infrastructure.Data.Entities;

class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ParentId { get; set; }
    public List<Category>? Children { get; set; }
    public List<ProductAttribute>? Attributes { get; set; }

    internal static class Schema
    {
        internal const string Table = "category";

        internal static string Id = nameof(Category.Id).ToSnakeCase();
        internal static string Name = nameof(Category.Name).ToSnakeCase();
        internal static string Description = nameof(Category.Description).ToSnakeCase();
        // internal static string ImageUrl = nameof(Category.ImageUrl).ToSnakeCase();
        internal static string ParentId = nameof(Category.ParentId).ToSnakeCase();
    }
}
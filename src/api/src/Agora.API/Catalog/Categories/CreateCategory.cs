namespace Agora.API.Catalog.Categories;

record CreateCategory(
    string Name,
    string? Description,
    int? ParentId,
    IList<ProductAttribute>? Attributes
);
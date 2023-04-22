namespace Agora.API.Catalog.Categories;

record CreateCategory(
    string Name,
    int? ParentId, // default 0?
    IList<ProductAttribute>? Attributes,
    string? Description = "",
    string? ImageUrl = "" // ? Or ID?
);

record ProductAttribute(
    string Name,
    bool PickOne,
    IList<string> Options
);
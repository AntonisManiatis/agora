using Agora.Catalog.Services.Categories;

namespace Agora.API.Catalog.Categories;

record CreateCategory(
    string Name,
    string? Description,
    string? ImageUrl, // ? Or ID?
    int? ParentId,
    IList<ProductAttribute>? Attributes
);
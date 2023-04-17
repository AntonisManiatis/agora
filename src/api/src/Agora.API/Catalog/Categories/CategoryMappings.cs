namespace Agora.API.Catalog.Categories;

static class CategoryMappings
{
    internal static void MapCategories(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/categories");
        group.MapPost("/", Categories.CreateAsync).RequireAuthorization(); // TODO: Using special policy (eg: Agora admin).
        // Related to images.
        // https://stackoverflow.com/questions/33279153/rest-api-file-ie-images-processing-best-practices
        group.MapPost("/{id}/image", Categories.UploadImageAsync).RequireAuthorization(); // TODO: Using special policy (eg: Agora admin).
        // TODO: or patch?
        group.MapPut("/{id}", Categories.UpdateAsync).RequireAuthorization(); // TODO: Using special policy (eg: Agora admin).
        group.MapDelete("/{id}", Categories.DeleteAsync).RequireAuthorization(); // TODO: Using special policy (eg: Agora admin).

        group.MapGet("/", Categories.GetAllAsync);
        group.MapGet("/{id}", Categories.GetAsync);
        group.MapGet("/{id}/attributes", Categories.GetAttributesAsync);
        // TODO: put or patch on a category's attributes?
    }
}
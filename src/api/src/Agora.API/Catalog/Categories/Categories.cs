namespace Agora.API.Catalog.Categories;

using Agora.Catalog.Services.Categories;

using Mapster;

using static Results;

static class Categories
{
    internal static void MapCategories(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/categories");
        group.MapPost("/", Categories.CreateAsync).RequireAuthorization(); // TODO: Using special policy (eg: Agora admin).
        // TODO: or patch?
        group.MapPut("/{id}", Categories.UpdateAsync).RequireAuthorization(); // TODO: Using special policy (eg: Agora admin).
        group.MapDelete("/{id}", Categories.DeleteAsync).RequireAuthorization(); // TODO: Using special policy (eg: Agora admin).

        group.MapGet("/", Categories.GetAllAsync);
        group.MapGet("/{id}", Categories.GetAsync).WithName("GetCategory");
        group.MapGet("/{id}/attributes", Categories.GetAttributesAsync);
        // TODO: put or patch on a category's attributes?
    }

    internal static async Task<IResult> CreateAsync(
        CreateCategory createCategory,
        ICategoryService categoryService
    )
    {
        var result = await categoryService.CreateAsync(
            createCategory.Adapt<CreateCategoryCommand>()
        );

        return result.MatchOk(
            id => CreatedAtRoute("GetCategory", new { Id = id }, id)
        );
    }

    internal static async Task<IResult> DeleteAsync(int id, ICategoryService categoryService)
    {
        var result = await categoryService.DeleteAsync(id);
        return result.MatchOk(
            _ => Ok()
        );
    }

    internal static Task<IResult> UpdateAsync(int id, ICategoryService categoryService) =>
        throw new NotImplementedException(); // TODO: Impl

    // ? Can I do the MVC thing here and return the type directly?
    internal static async Task<IResult> GetAllAsync(ICategoryService categoryService) =>
        Ok(await categoryService.GetAllAsync());

    internal static async Task<IResult> GetAsync(int id, ICategoryService categoryService)
    {
        var result = await categoryService.GetAsync(id);
        return result.MatchOk(
            category => Ok(category)
        );
    }

    internal static Task<IResult> GetAttributesAsync(int id, ICategoryService categoryService) =>
        throw new NotImplementedException(); // TODO: Impl
}
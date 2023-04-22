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

    static async Task<IResult> CreateAsync(
        CreateCategory createCategory,
        ICategoryService categoryService
    ) => await categoryService.CreateAsync(createCategory.Adapt<CreateCategoryCommand>())
        .MatchOkAsync(id => CreatedAtRoute("GetCategory", new { Id = id }, id));

    static async Task<IResult> DeleteAsync(int id, ICategoryService categoryService) =>
        await categoryService.DeleteAsync(id).MatchOkAsync(_ => Ok());

    static Task<IResult> UpdateAsync(int id, ICategoryService categoryService) =>
        throw new NotImplementedException(); // TODO: Impl

    static async Task<IResult> GetAllAsync(ICategoryService categoryService) =>
        Ok(await categoryService.GetAllAsync());

    static async Task<IResult> GetAsync(int id, ICategoryService categoryService) =>
        await categoryService.GetAsync(id).MatchOkAsync(category => Ok(category));

    static async Task<IResult> GetAttributesAsync(int id, ICategoryService categoryService) =>
        await categoryService.GetAttributesAsync(id).MatchOkAsync(attributes => Ok(attributes));
}
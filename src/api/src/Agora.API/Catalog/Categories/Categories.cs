namespace Agora.API.Catalog.Categories;

using Agora.Catalog.Services.Categories;

using Mapster;

using static Results;

static class Categories
{
    internal static async Task<IResult> CreateAsync(
        CreateCategory createCategory,
        ICategoryService categoryService
    )
    {
        var result = await categoryService.CreateAsync(
            createCategory.Adapt<CreateCategoryCommand>()
        );

        return result.MatchOk(
            id => Ok(id)
        );
    }

    internal static async Task<IResult> DeleteAsync(int id, ICategoryService categoryService)
    {
        var result = await categoryService.DeleteAsync(id);
        return result.MatchOk(
            _ => Ok()
        );
    }

    internal static Task<IResult> UploadImageAsync(int id, IFormFile file, ICategoryService categoryService) =>
        throw new NotImplementedException(); // TODO: Impl

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
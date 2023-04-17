using Agora.Catalog.Infrastructure.Data;
using Agora.Shared;

using ErrorOr;

using Mapster;

using CategoryEntity = Agora.Catalog.Infrastructure.Data.Entities.Category;

namespace Agora.Catalog.Services.Categories;

public record Category(
    int Id,
    string Name,
    string? Description,
    int? ParentId
);

public record Categories(
// TODO: All categories & nested, etc.
);

public record CreateCategoryCommand(
    string Name,
    string? Description,
    int? ParentId // TODO: attributes & options too?
);

public interface ICategoryService
{
    Task<ErrorOr<int>> CreateAsync(CreateCategoryCommand createCategory);

    // TODO: Upload image.

    Task<ErrorOr<Unit>> DeleteAsync(int id);

    Task<Categories> GetAllAsync();

    Task<ErrorOr<Category>> GetAsync(int id);
}

public static class Errors
{
    public static readonly Error ParentNotFound =
        Error.NotFound(code: "Parent.NotFound", description: "Parent category doesn't exist.");

    public static readonly Error CategoryNotFound =
        Error.NotFound(code: "Caregory.NotFound", description: "Category doesn't exist.");
}

// ! sealed might prevent the proxy? test it.
sealed class CategoryService : ICategoryService
{
    private readonly ICategoryRepository categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        this.categoryRepository = categoryRepository;
    }

    [Transactional]
    public async Task<ErrorOr<int>> CreateAsync(CreateCategoryCommand createCategory)
    {
        // TODO: validate command.

        if (createCategory.ParentId is int parentId && !await categoryRepository.ExistsAsync(parentId))
        {
            return Errors.ParentNotFound;
        }

        var id = await categoryRepository.NextIdentity();

        var category = new CategoryEntity
        {
            Id = id,
            Name = createCategory.Name,
            Description = createCategory.Description,
            ParentId = createCategory.ParentId
        };

        await categoryRepository.SaveAsync(category);

        return id;
    }

    [Transactional]
    public async Task<ErrorOr<Unit>> DeleteAsync(int id)
    {
        var category = await categoryRepository.GetAsync(id);
        if (category is null)
        {
            return Errors.CategoryNotFound;
        }

        // ? What happens if I delete a parent category? do the others stay orphan?
        // ? Can I even delete a parent? what are the rules?
        await categoryRepository.DeleteAsync(category);

        // TODO: should produce an event to eventually notify that products belonging to that category have been affected.

        return new Unit();
    }

    public Task<Categories> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ErrorOr<Category>> GetAsync(int id)
    {
        var category = await categoryRepository.GetAsync(id);
        if (category is null)
        {
            return Errors.CategoryNotFound;
        }

        return category.Adapt<Category>();
    }
}
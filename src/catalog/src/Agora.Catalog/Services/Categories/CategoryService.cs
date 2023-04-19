using Agora.Catalog.Infrastructure.Data;
using Agora.Shared;

using ErrorOr;

using FluentValidation;

using Mapster;

using CategoryEntity = Agora.Catalog.Infrastructure.Data.Entities.Category;

namespace Agora.Catalog.Services.Categories;

public record Category(
    int Id,
    string Name,
    string? Description = null,
    string? ImageUrl = null,
    int? ParentId = null,
    IList<Category>? Children = null
);

public record CreateCategoryCommand(
    string Name,
    string? Description = null,
    string? ImageUrl = null, // ? Or ID?
    int? ParentId = null,
    IList<ProductAttribute>? Attributes = null
);

public record ProductAttribute(
    string Name,
    bool PickOne,
    List<string> Options
);

sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        // ? Anything else?
        RuleFor(c => c.Name).NotEmpty().MaximumLength(3);
    }
}

public interface ICategoryService
{
    Task<ErrorOr<int>> CreateAsync(CreateCategoryCommand createCategory);

    Task<ErrorOr<Unit>> DeleteAsync(int id);

    Task<IEnumerable<Category>> GetAllAsync();

    Task<ErrorOr<Category>> GetAsync(int id);
}

public static class Errors
{
    public static readonly Error ParentNotFound =
        Error.NotFound(code: "Parent.NotFound", description: "Parent category doesn't exist.");

    public static readonly Error CategoryNotFound =
        Error.NotFound(code: "Caregory.NotFound", description: "Category doesn't exist.");

    public static readonly Error AlreadyExists =
        Error.Conflict(code: "Caregory.AlreadyExists", description: "Category already exists.");
}

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

        // ? can I avoid the 2 roundtrips here?
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
            // TODO: add attributes
        };

        var result = await categoryRepository.SaveAsync(category);
        if (result.IsError)
        {
            return result.Errors;
        }

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

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        var categories = await categoryRepository.GetAllAsync();
        return categories.Adapt<IEnumerable<Category>>();
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
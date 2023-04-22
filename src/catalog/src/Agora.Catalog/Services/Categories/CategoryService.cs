using Agora.Catalog.Infrastructure.Data;
using Agora.Shared;

using ErrorOr;

using FluentValidation;

using Mapster;

using CategoryEntity = Agora.Catalog.Infrastructure.Data.Entities.Category;
using ProductAttributeEntity = Agora.Catalog.Infrastructure.Data.Entities.ProductAttribute;
using ProductAttributes = System.Collections.Generic.IEnumerable<Agora.Catalog.Services.Categories.ProductAttribute>;
using ProductOptionEntity = Agora.Catalog.Infrastructure.Data.Entities.ProductOption;

namespace Agora.Catalog.Services.Categories;

public record Category(
    int Id, // TODO: Level or Depth would be good.
    string Name,
    string Description = "",
    string ImageUrl = "", // ? Or ID?
    int? ParentId = null, // ? Can this be default 0?
    IList<Category>? Children = null
);

public record CreateCategoryCommand(
    string Name,
    string Description = "",
    string ImageUrl = "", // ? Or ID?
    int? ParentId = null, // ? Can this be default 0?
    IList<ProductAttribute>? Attributes = null
);

sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        // ? Anything else?
        RuleFor(c => c.Name).NotEmpty().MaximumLength(3);

        // ! If there is an attribute, it cannot have an empty name or zero options.
    }
}

public record ProductAttribute(
    string Name,
    bool PickOne,
    IList<ProductOption> Options,
    int? Id = 0 // To reuse
);

public record ProductOption(
    string Name,
    int? Id = 0
)
{
    public static implicit operator ProductOption(string name) => new(name);

    public static implicit operator ProductOption((int, string) opt) => new(opt.Item2, opt.Item1);
}

public interface ICategoryService
{
    Task<ErrorOr<int>> CreateAsync(CreateCategoryCommand createCategory);

    Task<ErrorOr<Unit>> DeleteAsync(int id);

    Task<IEnumerable<Category>> GetAllAsync();

    Task<ErrorOr<Category>> GetAsync(int id);

    Task<ErrorOr<ProductAttributes>> GetAttributesAsync(int id);
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

        // TODO: I can use identity directly here actually.
        var id = await categoryRepository.NextIdentity();

        var category = new CategoryEntity
        {
            Id = id,
            Name = createCategory.Name,
            Description = createCategory.Description,
            ParentId = createCategory.ParentId,
        };

        if (createCategory.Attributes is not null or [])
        {
            category.Attributes = createCategory.Attributes!.Select(a => new ProductAttributeEntity
            {
                CategoryId = id,
                Name = a.Name,
                PickOne = a.PickOne,
                Options = a.Options.Select(option => new ProductOptionEntity { Name = option.Name }).ToList()
            }).ToList();
        }

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
        if (id <= 0)
        {
            return Errors.CategoryNotFound;
        }

        // ? this will generate a ton of garbage for something so simple. 
        // ? Maybe move the call to delete if there's no business logic.
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
        if (id <= 0)
        {
            return Errors.CategoryNotFound;
        }

        var category = await categoryRepository.GetAsync(id);
        if (category is null)
        {
            return Errors.CategoryNotFound;
        }

        return category.Adapt<Category>();
    }

    public async Task<ErrorOr<ProductAttributes>> GetAttributesAsync(int id)
    {
        if (id <= 0)
        {
            return Errors.CategoryNotFound;
        }

        var category = await categoryRepository.GetAsync(id, plusAttributes: true);
        if (category is null)
        {
            return Errors.CategoryNotFound;
        }

        if (category.Attributes is null)
        {
            return Errors.CategoryNotFound;
        }

        return ErrorOrFactory.From(category.Attributes.Adapt<ProductAttributes>());
    }
}
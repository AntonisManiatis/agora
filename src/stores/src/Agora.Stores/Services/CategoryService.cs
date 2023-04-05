using Agora.Stores.Core.Stores;

using ErrorOr;

namespace Agora.Stores.Services;

public record MakeCategoryCommand(
    Guid StoreId,
    string Name
);

public record CategoryDTO(
    int Id,
    string Name
);

public interface ICategoryService
{
    // TODO: Returns?
    Task<ErrorOr<int>> MakeCategoryAsync(MakeCategoryCommand command);

    // TODO: Delete a category
    // TODO: Rename a category
    // TODO: Add or remove products from a category.
}

sealed class CategoryService : ICategoryService
{
    private readonly IStoreRepository storeRepository;

    public CategoryService(IStoreRepository storeRepository)
    {
        this.storeRepository = storeRepository;
    }

    public async Task<ErrorOr<int>> MakeCategoryAsync(MakeCategoryCommand command)
    {
        var store = await storeRepository.GetStoreAsync(command.StoreId);
        if (store is null)
        {
            return Errors.Stores.NotFound;
        }

        if (store.HasCategory(command.Name))
        {
            return Errors.Categories.AlreadyExists;
        }

        store.AddCategory(command.Name);

        _ = await storeRepository.AddAsync(store);

        // ? I don't like this, Do I really need to return a cat id?
        var categoryId = store.Categories
            .Where(x => x.Name == command.Name)
            .Select(x => x.Id)
            .FirstOrDefault();

        return categoryId;
    }
}
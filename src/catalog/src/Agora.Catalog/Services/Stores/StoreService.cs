using Agora.Catalog.Infrastructure.Data;
using Agora.Catalog.Infrastructure.Data.Entities;
using Agora.Shared;

using ErrorOr;

namespace Agora.Catalog.Services.Stores;

public interface IStoreService // TODO: Naming..
{
    Task<ErrorOr<Unit>> ListStoreAsync(ListStoreCommand command);
}

public static class Errors
{
    public static Error StoreAlreadyExists =
        Error.Conflict(code: "Stores.AlreadyExists", description: $"A store with the same name already exists.");
}

sealed class StoreService : IStoreService
{
    private const string DefaultLang = "en";

    private readonly IStoreRepository storeRepository;

    public StoreService(IStoreRepository storeRepository)
    {
        this.storeRepository = storeRepository;
    }

    public async Task<ErrorOr<Unit>> ListStoreAsync(ListStoreCommand command)
    {
        // TODO: add validator.

        var exists = await storeRepository.ExistsAsync(command.Name);
        if (exists)
        {
            return Errors.StoreAlreadyExists;
        }

        var store = new Store
        {
            Id = command.Id,
            Name = command.Name,
            Lang = command.Lang ?? DefaultLang // TODO: We'll get back to this, this should be configurable and dependant on installation
        }; // TODO: Store is not active yet.

        await storeRepository.SaveAsync(store);

        return new Unit();
    }
}
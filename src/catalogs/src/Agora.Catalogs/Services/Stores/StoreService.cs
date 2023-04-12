using Agora.Catalogs.Infrastructure.Data;
using Agora.Catalogs.Infrastructure.Data.Entities;
using Agora.Shared;

using ErrorOr;

namespace Agora.Catalogs.Services.Stores;

public interface IStoreService // TODO: Naming..
{
    Task<ErrorOr<Unit>> ListStoreAsync(ListStoreCommand command);
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
        var exists = await storeRepository.ExistsAsync(command.Name);
        if (exists)
        {
            return Errors.StoreAlreadyExists(command.Name);
        }

        var store = new Store(
            command.Id,
            command.Name,
            command.Lang ?? DefaultLang // TODO: We'll get back to this, this should be configurable and dependant on installation
        ); // TODO: Store is not active yet.

        await storeRepository.SaveAsync(store);

        return new Unit();
    }
}
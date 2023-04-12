using Agora.Shared;
using Agora.Stores.Core.Stores;

using ErrorOr;

using Mapster;

namespace Agora.Stores.Services;

public record Store
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    // ? or int?
    public string Status { get; init; } = string.Empty;
    // TODO: What else?
}

public record RegisterStoreCommand(
    Guid StoreId,
    Guid OwnerId
);

public record TaxAddr
{
    public static readonly TaxAddr Undefined = new();

    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
}

public interface IStoreService
{
    Task<ErrorOr<Store>> GetStoreAsync(Guid storeId);

    Task<IEnumerable<Store>> GetStoresAsync();

    Task<ErrorOr<Unit>> RegisterStoreAsync(RegisterStoreCommand command);
}

sealed class StoreService : IStoreService
{
    private readonly IStoreRepository storeRepository;

    public StoreService(IStoreRepository storeRepository)
    {
        this.storeRepository = storeRepository;
    }

    public async Task<ErrorOr<Store>> GetStoreAsync(Guid storeId)
    {
        var store = await storeRepository.GetStoreAsync(storeId);
        if (store is null)
        {
            return Error.NotFound();
        }

        return store.Adapt<Store>();
    }

    public Task<IEnumerable<Store>> GetStoresAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ErrorOr<Unit>> RegisterStoreAsync(RegisterStoreCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        // TODO: Add "superficial" validation here.
        // TODO: Also I'll see if I can make a decorator and register it to DI so that it always validates a request

        // ? Is there a chance that a user cannot open multile stores? 

        var store = Core.Stores.Store.Create(
            command.StoreId,
            command.OwnerId
        );

        await storeRepository.AddAsync(store);

        return new Unit();
    }
}
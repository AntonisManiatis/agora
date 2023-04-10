using Agora.Shared.Infrastructure.Messaging;
using Agora.Stores.Contracts;
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

public record OpenStoreCommand
{
    public Guid UserId { get; set; }

    public string Name { get; init; } = string.Empty;
    public TaxAddr TaxAddr { get; init; } = TaxAddr.Undefined;
    // ? Or Tax Identification Number 
    public string Tin { get; init; } = string.Empty;
    // AKA GEMI in Greece.
    public string? Brn { get; init; } // ? Could be specified later if it's not required immediately.
}

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

    Task<ErrorOr<Guid>> OpenStoreAsync(OpenStoreCommand command);
}

sealed class StoreService : IStoreService
{
    private readonly IStoreRepository storeRepository;
    private readonly IMessagePublisher publisher;

    public StoreService(
        IStoreRepository storeRepository,
        IMessagePublisher publisher)
    {
        this.storeRepository = storeRepository;
        this.publisher = publisher;
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

    public async Task<ErrorOr<Guid>> OpenStoreAsync(OpenStoreCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        // TODO: Add "superficial" validation here.
        // TODO: Also I'll see if I can make a decorator and register it to DI so that it always validates a request

        var exists = await storeRepository.ExistsAsync(command.Name);
        if (exists)
        {
            return Error.Conflict(description: $"A store named {command.Name} already exists.");
        }
        // ? Is there a chance that a user cannot open multile stores? 

        var store = new Core.Stores.Store
        {
            UserId = command.UserId,
            Name = command.Name,
            Tin = command.Tin,
            // TaxAddress = command.TaxAddr.Adapt<Core.Stores.TaxAddress>() // TODO: Mapster fails because immutable, will fix later
        };

        // Save to DB.
        var storeId = await storeRepository.AddAsync(store);

        await publisher.PublishAsync(new StoreOpened(command.UserId, storeId));

        return storeId;
    }
}
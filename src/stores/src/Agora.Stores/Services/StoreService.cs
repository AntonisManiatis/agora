using Agora.Shared.Infrastructure.Data;
using Agora.Shared.Infrastructure.Messaging;
using Agora.Stores.Contracts;
using Agora.Stores.Core;
using Agora.Stores.Infrastructure.Data;

using Dapper;

using ErrorOr;

using Mapster;

namespace Agora.Stores.Services;

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
    Task<ErrorOr<StoreDTO>> GetStoreAsync(Guid storeId);

    Task<IEnumerable<StoreDTO>> GetStoresAsync();

    Task<ErrorOr<Guid>> OpenStoreAsync(OpenStoreCommand req);
}

internal class StoreService : IStoreService
{
    // TODO: Consider using repositories since we're not using EFCore.
    private readonly IDbConnector connector;
    private readonly IMessagePublisher publisher;

    public StoreService(IDbConnector connector, IMessagePublisher publisher)
    {
        this.connector = connector;
        this.publisher = publisher;
    }

    public async Task<ErrorOr<StoreDTO>> GetStoreAsync(Guid storeId)
    {
        using var connection = await connector.ConnectAsync();

        var store = await connection.QueryFirstOrDefaultAsync<Store>(
            $"SELECT * FROM {Sql.Stores.Table} WHERE id = @Id",
            new { Id = storeId }
        );
        if (store is null)
        {
            return Error.NotFound();
        }

        return store.Adapt<StoreDTO>();
    }

    public async Task<IEnumerable<StoreDTO>> GetStoresAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ErrorOr<Guid>> OpenStoreAsync(OpenStoreCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        // TODO: Add "superficial" validation here.
        // TODO: Also I'll see if I can make a decorator and register it to DI so that it always validates a request

        using var connection = await connector.ConnectAsync();

        var exists = await connection.ExecuteScalarAsync<bool>(
            $"SELECT COUNT(1) FROM {Sql.Stores.Table} WHERE name=@Name",
            new { Name = command.Name }
        );
        if (exists)
        {
            return Error.Conflict(description: $"A store named {command.Name} already exists.");
        }

        // ? Is there a chance that a user cannot open multile stores? 

        var store = new Store
        {
            UserId = command.UserId,
            Name = command.Name,
            Tin = command.Tin,
            TaxAddress = command.TaxAddr.Adapt<TaxAddress>()
        };

        // Save to DB.
        var storeId = await connection.ExecuteScalarAsync<Guid>(
            $"INSERT INTO {Sql.Stores.Table} (user_id, name, status, tin) VALUES (@UserId, @Name, @Status, @Tin) RETURNING id",
            new
            {
                UserId = store.UserId,
                Name = store.Name,
                Status = store.Status,
                Tin = store.Tin
            }
        );

        await publisher.PublishAsync(new StoreOpened(command.UserId, storeId));

        return storeId;
    }
}

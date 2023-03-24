using Agora.Shared.Infrastructure.Data;
using Agora.Stores.Core;
using Agora.Stores.Infrastructure.Data;

using Dapper;

using ErrorOr;

namespace Agora.Stores;

public record OpenStoreRequest
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public StoreAddr StoreAddr { get; init; } = StoreAddr.None;
}

public record StoreAddr
{
    public static readonly StoreAddr None = new();

    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
}

// TODO: Consider AutoMapper or Mapster.
internal static class StoreAddrExtensions
{
    internal static Address ToCore(this StoreAddr storeAddr)
    {
        return new Address
        {
            Street = storeAddr.Street,
            City = storeAddr.City,
            State = storeAddr.State,
            ZipCode = storeAddr.ZipCode
        };
    }
}

public record OpenStoreResponse(Guid Id);

public class StoreService
{
    private readonly IDbConnector connector;

    public StoreService(IDbConnector connector)
    {
        this.connector = connector;
    }

    public async Task<ErrorOr<Guid>> SubmitOpenStoreRequestAsync(OpenStoreRequest req)
    {
        ArgumentNullException.ThrowIfNull(req);

        using var connection = await connector.ConnectAsync();
        // TODO: validate if name already exists, etc.

        // TODO: Create the request.
        var application = new StoreApplication
        {
            User = req.UserId,
            Name = req.Name,
            Address = req.StoreAddr.ToCore()
        };

        // TODO: Raise an event saying a request was made.

        // Save to DB.
        var id = await connection.ExecuteScalarAsync<Guid>(
            $"INSERT INTO {Sql.StoreApplications.Table} (name) VALUES (@Name) RETURNING id", new { Name = application.Name });

        return id;
    }
}

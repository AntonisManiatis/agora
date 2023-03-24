using Agora.Shared.Infrastructure.Data;
using Agora.Stores.Core;
using Agora.Stores.Infrastructure.Data;

using Dapper;

using ErrorOr;

using Mapster;

namespace Agora.Stores;

// TODO: How should we do superficial validation? DataComponent or FluentValidator?
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

public class StoreService
{
    private readonly IDbConnector connector;

    public StoreService(IDbConnector connector)
    {
        this.connector = connector;
    }

    // TODO: perhaps userId too?
    public async Task<ErrorOr<StoreApplicationDTO>> GetApplication(Guid applicationId)
    {
        using var connection = await connector.ConnectAsync();

        var application = await connection.QueryFirstOrDefaultAsync<StoreApplication>(
            $"SELECT * FROM {Sql.StoreApplications.Table} WHERE id = @Id",
            new { Id = applicationId }
        );
        if (application is null)
        {
            return Error.NotFound();
        }

        return application.Adapt<StoreApplicationDTO>();
    }

    public async Task<ErrorOr<Guid>> SubmitOpenStoreRequestAsync(OpenStoreRequest req)
    {
        ArgumentNullException.ThrowIfNull(req);

        using var connection = await connector.ConnectAsync();

        // * Depends on the business logic, ideally it shouldn't collide with current shop names.
        // * and not currently not Approved or Rejected applications.
        var exists = await connection.ExecuteScalarAsync<bool>(
            $"SELECT COUNT(1) FROM {Sql.StoreApplications.Table} WHERE name=@Name",
            new { Name = req.Name }
        );
        if (exists)
        {
            return Error.Conflict(description: $"A store named {req.Name} already exists.");
        }

        // ? Is a physical address required?

        // ? Is there a chance that a user cannot submit multiple independant requests?

        var application = new StoreApplication
        {
            UserId = req.UserId,
            Name = req.Name,
            Address = req.StoreAddr.Adapt<Address>()
        };

        // Save to DB.
        var id = await connection.ExecuteScalarAsync<Guid>(
            $"INSERT INTO {Sql.StoreApplications.Table} (name) VALUES (@Name) RETURNING id",
            new { Name = application.Name }
        );

        // TODO: Raise an event saying a request was made.

        return id;
    }
}

using Agora.Shared.Infrastructure.Data;
using Agora.Shared.Infrastructure.Messaging;
using Agora.Stores.Contracts;
using Agora.Stores.Core;
using Agora.Stores.Infrastructure.Data;

using Dapper;

using ErrorOr;

using Mapster;

namespace Agora.Stores.Services;

public record OpenStoreRequest
{
    public Guid UserId { get; init; }

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

public class StoreService
{
    // TODO: Consider using repositories since we're not using EFCore.
    private readonly IDbConnector connector;
    private readonly IMessagePublisher publisher;

    public StoreService(IDbConnector connector, IMessagePublisher publisher)
    {
        this.connector = connector;
        this.publisher = publisher;
    }

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
        // TODO: Add "superficial" validation here.
        // TODO: Also I'll see if I can make a decorator and register it to DI so that it always validates a request

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

        // ? Is there a chance that a user cannot submit multiple independant requests?

        var application = new StoreApplication
        {
            UserId = req.UserId,
            Name = req.Name,
            Tin = req.Tin,
            TaxAddress = req.TaxAddr.Adapt<TaxAddress>()
        };

        // Save to DB.
        var id = await connection.ExecuteScalarAsync<Guid>(
            $"INSERT INTO {Sql.StoreApplications.Table} (user_id, name, status, tin) VALUES (@UserId, @Name, @Status, @Tin) RETURNING id",
            new
            {
                UserId = application.UserId,
                Name = application.Name,
                Status = application.Status,
                Tin = application.Tin
            }
        );

        await publisher.PublishAsync(new ApplicationSubmitted(req.UserId, id));

        return id;
    }
}

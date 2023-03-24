using Dapper;

using ErrorOr;

namespace Agora.Stores.IntegrationTests;

[Collection(nameof(PostgreSqlFixture))]
public class StoreServiceTest
{
    private readonly PostgreSqlFixture fixture;

    public StoreServiceTest(PostgreSqlFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Returns_an_application_id_if_the_request_is_valid()
    {
        // Arrange
        var storeService = fixture.Service;

        var req = new OpenStoreRequest
        {
            UserId = Guid.NewGuid(),
            Name = "Coffee Lab",
            StoreAddr = StoreAddr.None // TODO: Probably not a good idea but temp.
        };

        // Act
        var result = await storeService.SubmitOpenStoreRequestAsync(req);

        // Assert
        Assert.NotEqual(default, result.Value);

    }

    [Fact]
    public async Task Returns_an_error_if_the_provided_store_name_already_exists()
    {
        // Arrange
        const string Name = "Pizza Palace";

        using var connection = await fixture.Connector.ConnectAsync();
        await connection.ExecuteAsync($"INSERT INTO store_request (name) VALUES ('{Name}')");

        var storeService = fixture.Service;

        var req = new OpenStoreRequest
        {
            UserId = Guid.NewGuid(),
            Name = Name,
            StoreAddr = StoreAddr.None
        };

        // Act
        var result = await storeService.SubmitOpenStoreRequestAsync(req);

        // Assert
        // TODO: I hate having this test depend on the description.
        Assert.Contains(Error.Conflict(description: $"A store named {Name} already exists."), result.Errors);
    }

    [Fact]
    public async Task Retrieving_an_application_that_does_not_exist_returns_an_error()
    {
        // Arrange
        var storeService = fixture.Service;
        var applicationId = Guid.NewGuid();

        // Act
        var result = await storeService.GetApplication(applicationId);

        // Assert
        Assert.Contains(Error.NotFound(), result.Errors);
    }

    [Fact]
    public async Task An_application_is_in_pending_state_after_being_sumbitted()
    {
        // Arrange
        var storeService = fixture.Service;
        var req = new OpenStoreRequest { UserId = Guid.NewGuid(), Name = "My store" };
        var applicationId = await storeService.SubmitOpenStoreRequestAsync(req);

        // Act
        var result = await storeService.GetApplication(applicationId.Value);

        // Assert
        var expected = new StoreApplicationDTO
        {
            Name = "My store",
            Status = "Pending"
        };

        Assert.Equal(expected, result.Value);
    }
}
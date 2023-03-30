using Agora.Stores.Services;

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
    public async Task Returns_a_store_id_if_the_request_is_valid()
    {
        // Arrange
        var storeService = fixture.Service;

        var req = new OpenStoreRequest
        {
            UserId = Guid.NewGuid(),
            Name = "Coffee Lab",
            Tin = "000000000",
            TaxAddr = TaxAddr.Undefined // TODO: Probably not a good idea but temp.
        };

        // Act
        var result = await storeService.OpenStoreAsync(req);

        // Assert
        Assert.NotEqual(default, result.Value);

    }

    [Fact]
    public async Task Returns_an_error_if_the_provided_store_name_already_exists()
    {
        // Arrange
        const string Name = "Pizza Palace";

        using var connection = await fixture.Connector.ConnectAsync();
        var userId = Guid.NewGuid();
        await connection.ExecuteAsync($"INSERT INTO store (user_id, name, tin) VALUES ('{userId}', '{Name}', '1')");

        var storeService = fixture.Service;

        var req = new OpenStoreRequest
        {
            UserId = Guid.NewGuid(),
            Name = Name,
            Tin = "000000000",
            TaxAddr = TaxAddr.Undefined
        };

        // Act
        var result = await storeService.OpenStoreAsync(req);

        // Assert
        // TODO: I hate having this test depend on the description.
        Assert.Contains(Error.Conflict(description: $"A store named {Name} already exists."), result.Errors);
    }

    [Fact]
    public async Task Retrieving_a_store_that_does_not_exist_returns_an_error()
    {
        // Arrange
        var storeService = fixture.Service;
        var applicationId = Guid.NewGuid();

        // Act
        var result = await storeService.GetStoreAsync(applicationId);

        // Assert
        Assert.Contains(Error.NotFound(), result.Errors);
    }

    [Fact]
    public async Task An_application_is_in_pending_state_after_being_sumbitted()
    {
        // Arrange
        var storeService = fixture.Service;
        var req = new OpenStoreRequest
        {
            UserId = Guid.NewGuid(),
            Name = "My store",
            Tin = "000000000"
        };

        var applicationId = await storeService.OpenStoreAsync(req);

        // Act
        var result = await storeService.GetStoreAsync(applicationId.Value);

        // Assert
        var expected = new StoreDTO
        {
            Id = applicationId.Value,
            Name = "My store",
            Status = "Pending"
        };

        Assert.Equal(expected, result.Value);
    }
}
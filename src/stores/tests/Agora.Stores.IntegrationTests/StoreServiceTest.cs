using Agora.Stores.Services;

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
    public async Task Returns_unit_if_the_request_is_valid()
    {
        // Arrange
        var storeService = fixture.StoreService;

        var command = new RegisterStoreCommand(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Act
        var result = await storeService.RegisterStoreAsync(command);

        // Assert
        Assert.Equal(default, result.Value);
    }

    /* // TODO: Move to catalogs
    [Fact]
    public async Task Returns_an_error_if_the_provided_store_name_already_exists()
    {
        // Arrange
        const string Name = "Pizza Palace";

        using var connection = await fixture.Connector.ConnectAsync();
        var userId = Guid.NewGuid();
        await connection.ExecuteAsync($"INSERT INTO stores.store (user_id, name, tin) VALUES ('{userId}', '{Name}', '1')");

        var storeService = fixture.StoreService;

        var command = new RegisterStoreCommand
        {
            OwnerId = Guid.NewGuid(),
            Name = Name,
        };

        // Act
        var result = await storeService.RegisterStoreAsync(command);

        // Assert
        // TODO: I hate having this test depend on the description.
        Assert.Contains(Error.Conflict(description: $"A store named {Name} already exists."), result.Errors);
    }
    */

    [Fact]
    public async Task Retrieving_a_store_that_does_not_exist_returns_an_error()
    {
        // Arrange
        var storeService = fixture.StoreService;
        var applicationId = Guid.NewGuid();

        // Act
        var result = await storeService.GetStoreAsync(applicationId);

        // Assert
        Assert.Contains(Error.NotFound(), result.Errors);
    }
}
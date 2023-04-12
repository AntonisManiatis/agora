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
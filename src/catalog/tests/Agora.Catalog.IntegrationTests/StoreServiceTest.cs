using Agora.Catalog.Services.Stores;

namespace Agora.Catalog.IntegrationTests;

[Collection("Catalog")]
public class StoreServiceTest
{
    private readonly ServiceFixture fixture;

    public StoreServiceTest(ServiceFixture fixture) => this.fixture = fixture;

    [Fact]
    public async Task Returns_an_error_if_the_provided_store_name_already_exists()
    {
        // Arrange
        const string name = "Pizza Palace";

        using var scope = fixture.StoreService;
        var service = scope.Service;

        await service.ListStoreAsync(new ListStoreCommand(
           Guid.NewGuid(),
           name,
           "en"
        ));

        var command = new ListStoreCommand(
           Guid.NewGuid(),
           name,
           "en"
        );

        // Act
        var result = await service.ListStoreAsync(command);

        // Assert
        Assert.Contains(Errors.StoreAlreadyExists, result.Errors);
    }
}
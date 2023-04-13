using Agora.Catalogs.Services.Stores;

namespace Agora.Catalogs.IntegrationTests;

[Collection("Catalog")]
public class StoreServiceTest
{
    private readonly ServiceFixture fixture;

    public StoreServiceTest(ServiceFixture fixture) => this.fixture = fixture;

    [Fact]
    public async Task Returns_an_error_if_the_provided_store_name_already_exists()
    {
        // Arrange
        const string Name = "Pizza Palace";

        using var scope = fixture.StoreService;
        var service = scope.Service;

        await service.ListStoreAsync(new ListStoreCommand(
           Guid.NewGuid(),
           Name,
           "en"
        ));

        var command = new ListStoreCommand(
           Guid.NewGuid(),
           Name,
           "en"
        );

        // Act
        var result = await service.ListStoreAsync(command);

        // Assert
        Assert.Contains(Errors.StoreAlreadyExists(Name), result.Errors);
    }
}
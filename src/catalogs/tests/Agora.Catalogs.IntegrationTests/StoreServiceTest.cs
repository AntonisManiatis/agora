using Agora.Catalogs.Services.Stores;

using ErrorOr;

namespace Agora.Catalogs.IntegrationTests;

[Collection("Catalog")]
public class StoreServiceTest : IClassFixture<StoreServiceFixture>
{
    private readonly StoreServiceFixture fixture;

    public StoreServiceTest(StoreServiceFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Returns_an_error_if_the_provided_store_name_already_exists()
    {
        // Arrange
        const string Name = "Pizza Palace";

        var service = fixture.Service;

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
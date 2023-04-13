using Agora.Stores.Services;

namespace Agora.Stores.IntegrationTests;

[Collection("Stores")]
public class InventoryServiceTest : IAsyncLifetime
{
    private readonly ServiceFixture fixture;
    private Guid existingStoreId;

    public InventoryServiceTest(ServiceFixture fixture) => this.fixture = fixture;

    [Fact]
    public async Task Providing_any_of_the_following_args_returns_a_validation_error()
    {
        // Arrange
        using var scope = fixture.InventoryService;
        var inventoryService = scope.Service;
        var command = new ListProductCommand(
            Guid.Empty,
            Guid.Empty,
            "101-SB",
            0
        );

        // Act
        var result = await inventoryService.AddListingAsync(command);

        // Assert
        var codes = result.Errors.Select(err => err.Code);
        Assert.Contains(nameof(ListProductCommand.StoreId), codes);
        Assert.Contains(nameof(ListProductCommand.ProductId), codes);
        Assert.Contains(nameof(ListProductCommand.Quantity), codes);
    }

    [Fact]
    public async Task Adding_a_listing_to_a_non_existing_store_returns_an_error()
    {
        // Arrange
        using var scope = fixture.InventoryService;
        var inventoryService = scope.Service;

        var storeId = Guid.NewGuid(); // Any random guid that doesn't exist
        var command = new ListProductCommand(
            Guid.NewGuid(),
            storeId,
            "101-SB",
            10
        );

        // Act
        var result = await inventoryService.AddListingAsync(command);

        // Assert
        Assert.Contains(Errors.Stores.NotFound, result.Errors);
    }

    [Fact]
    public async Task Add() // TODO: Name
    {
        // Arrange
        using var scope = fixture.InventoryService;
        var inventoryService = scope.Service;

        var productId = Guid.NewGuid();
        var command = new ListProductCommand(
            productId,
            existingStoreId,
            "101-SB",
            10
        );

        // Act
        var result = await inventoryService.AddListingAsync(command);

        // Assert
        var expected = new Product(
            productId,
            existingStoreId,
            "101-SB",
            10
        );
        var actual = await inventoryService.GetProductAsync(productId);

        Assert.Equal(expected, actual.Value);
    }

    public async Task InitializeAsync()
    {
        var storeId = Guid.NewGuid();
        var command = new RegisterStoreCommand(
            storeId,
            Guid.NewGuid()
        );

        using var scope = fixture.StoreService;
        var result = await scope.Service.RegisterStoreAsync(command);
        this.existingStoreId = storeId;
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
using Agora.Stores.Services;

namespace Agora.Stores.IntegrationTests;

[Collection(nameof(PostgreSqlFixture))]
public class ProductServiceTest : IAsyncLifetime
{
    private readonly PostgreSqlFixture fixture;
    private Guid existingStoreId;

    public ProductServiceTest(PostgreSqlFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Providing_any_of_the_following_args_returns_a_validation_error()
    {
        // Arrange
        var productService = fixture.ProductService;
        var command = new ListProductCommand(
            Guid.Empty,
            Guid.Empty,
            "101-SB",
            0
        );

        // Act
        var result = await productService.AddListingAsync(command);

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
        var productService = fixture.ProductService;

        var storeId = Guid.NewGuid(); // Any random guid that doesn't exist
        var command = new ListProductCommand(
            Guid.NewGuid(),
            storeId,
            "101-SB",
            10
        );

        // Act
        var result = await productService.AddListingAsync(command);

        // Assert
        Assert.Contains(Errors.Stores.NotFound, result.Errors);
    }

    [Fact]
    public async Task Add() // TODO: Name
    {
        // Arrange
        var productService = fixture.ProductService;

        var productId = Guid.NewGuid();
        var command = new ListProductCommand(
            productId,
            existingStoreId,
            "101-SB",
            10
        );

        // Act
        var result = await productService.AddListingAsync(command);

        // Assert
        var expected = new Product(
            productId,
            existingStoreId,
            "101-SB",
            10
        );
        var actual = await productService.GetProductAsync(productId);

        Assert.Equal(expected, actual.Value);
    }

    public async Task InitializeAsync()
    {
        var command = new OpenStoreCommand
        {
            UserId = Guid.NewGuid(),
            Name = "Testing inventory",
            Tin = "000000000",
            TaxAddr = TaxAddr.Undefined
        };

        var result = await fixture.StoreService.OpenStoreAsync(command);
        this.existingStoreId = result.Value;
    }

    public Task DisposeAsync() => Task.CompletedTask;
}